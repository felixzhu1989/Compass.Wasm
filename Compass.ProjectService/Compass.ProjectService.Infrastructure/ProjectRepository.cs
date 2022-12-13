using Compass.ProjectService.Domain;
using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared;
using Microsoft.EntityFrameworkCore;

namespace Compass.ProjectService.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _context;
    public ProjectRepository(ProjectDbContext context)
    {
        _context = context;
    }
    #region Project
    public Task<PaginationResult<IQueryable<Project>>> GetProjectsAsync(int page)
    {
        var pageResults = 15f;//默认一页显示数据条数
        var pageCount = Math.Ceiling(_context.Projects.Count() / pageResults);//计算页总数
        return Task.FromResult(new PaginationResult<IQueryable<Project>>
        {
            Data = _context.Projects
                .OrderByDescending(x => x.DeliveryDate)
                .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
                .Take((int)pageResults),
            CurrentPage = page,
            Pages = (int)pageCount
        });
    }
    public Task<Project?> GetProjectByIdAsync(Guid id)
    {
        return _context.Projects.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<Project?> GetProjectByOdpAsync(string odpNumber)
    {
        return _context.Projects.SingleOrDefaultAsync(x => x.OdpNumber.Contains(odpNumber));
    }

    public async Task<string> GetOdpNumberByIdAsync(Guid id)
    {
        var project = await _context.Projects.SingleOrDefaultAsync(x => x.Id.Equals(id));
        return project.OdpNumber;
    }

    public async Task<DateTime> GetDeliveryDateByIdAsync(Guid id)
    {
        var project = await _context.Projects.SingleOrDefaultAsync(x => x.Id.Equals(id));
        return project.DeliveryDate;
    }

    public Task<IQueryable<Project>> GetUnbindProjectsAsync(List<Guid?> ids)
    {
       return Task.FromResult(_context.Projects.Where(x => !ids.Contains(x.Id)));
    }

    #endregion

    #region Drawing
    public Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Drawings.Where(x => x.ProjectId.Equals(projectId)).OrderBy(x=>x.ItemNumber).AsQueryable());
    }

    public Task<IQueryable<Drawing>> GetDrawingsByUserIdAsync(Guid userId)
    {
        return Task.FromResult(_context.Drawings.Where(x => x.UserId.Equals(userId)).OrderByDescending(x=>x.CreationTime).AsQueryable());
    }

    public Task<Drawing?> GetDrawingByIdAsync(Guid id)
    {
        return _context.Drawings.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    public Task<bool> DrawingExistsInProjectAsync(Guid projectId)
    {
        return _context.Drawings.AnyAsync(x => x.ProjectId.Equals(projectId));
    }
    public Task<int> GetTotalDrawingsCountInProjectAsync(Guid projectId)
    {
        return _context.Drawings.CountAsync(x => x.ProjectId.Equals(projectId));
    }

    public Task<int> GetNotAssignedDrawingsCountInProjectAsync(Guid projectId)
    {
        return _context.Drawings.CountAsync(x => x.ProjectId.Equals(projectId)&& x.UserId == null || x.UserId.Equals(Guid.Empty));
    }

    #endregion

    #region Module
    public Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId)
    {
        return Task.FromResult(_context.Modules.Where(x => x.DrawingId.Equals(drawingId)).OrderBy(x=>x.Name).AsQueryable());
    }
    

    public Task<Module?> GetModuleByIdAsync(Guid id)
    {
        return _context.Modules.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<bool> ModuleExistsInDrawing(Guid drawingId)
    {
        return _context.Modules.AnyAsync(x => x.DrawingId.Equals(drawingId));
    }
   

    #endregion

    #region DrawingPlan

    public Task<PaginationResult<IQueryable<DrawingPlan>>> GetDrawingPlansAsync(int page)
    {
        var pageResults = 5f;//默认一页显示数据条数
        var pageCount = Math.Ceiling(_context.DrawingsPlan.Count() / pageResults);//计算页总数
        return Task.FromResult(new PaginationResult<IQueryable<DrawingPlan>>
        {
            Data = _context.DrawingsPlan
                .OrderByDescending(x => x.ReleaseTime)
                .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
                .Take((int)pageResults),
                
            CurrentPage = page,
            Pages = (int)pageCount
        });
    }

    public Task<DrawingPlan?> GetDrawingPlanByIdAsync(Guid id)
    {
        return _context.DrawingsPlan.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    

    public async Task<IEnumerable<Project>> GetProjectsNotDrawingPlannedAsync()
    {
        var projects =await _context.Projects.ToListAsync();//所有项目
        var plannedProjects =await _context.DrawingsPlan.ToListAsync() ;//所有制图计划
        var notDrawingPlannedProjects = projects.Where(x => !plannedProjects.Exists(dp => x.Id.Equals(dp.Id)));
        return notDrawingPlannedProjects;
    }

    public async Task<bool> IsDrawingsNotAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        return drawings.Any(x => x.UserId == null || x.UserId.Equals(Guid.Empty));
    }

    public async Task<IEnumerable<Drawing>> GetDrawingsNotAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        var notAssignedDrawings = new List<Drawing>();
        foreach (var drawing in drawings)
        {
            if (drawing.UserId == null)//空的
            {
                notAssignedDrawings.Add(drawing);
            }
            else if (drawing.UserId.Equals(Guid.Empty))//全部是0
            {
                notAssignedDrawings.Add(drawing);
            }
        }
        return notAssignedDrawings;
    }

    public async Task<Dictionary<Guid, IQueryable<Drawing>>> GetDrawingsAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        var userIds = drawings.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.UserId).Distinct();
        var assignedDrawings = new Dictionary<Guid, IQueryable<Drawing>>();
        if (userIds != null && userIds.Count()!=0)
        {
            //userIds.OfType<Guid>()
            foreach (var userId in userIds)
            {
                var items = drawings.Where(x => x.UserId.Equals(userId));
                assignedDrawings.Add(userId.GetValueOrDefault(), items);
            }
        }
        return assignedDrawings;
    }

    public async Task AssignDrawingsToUserAsync(IEnumerable<Guid> drawingIds, Guid userId)
    {
        foreach (var drawingId in drawingIds)
        {
            var dbDrawing = await GetDrawingByIdAsync(drawingId);
            dbDrawing.ChangeUserId(userId);
        }
    }

    #endregion

    #region Tracking
    public async Task<PaginationResult<IQueryable<Tracking>>> GetTrackingsAsync(int page)
    {
        var pageResults = 10f;//默认一页显示数据条数
        var pageCount = Math.Ceiling(_context.Trackings.Count() / pageResults);//计算页总数
        return new PaginationResult<IQueryable<Tracking>>
        {
            /* 使用查询子句先修改掉排序日期
             * update Trackings set Trackings.SortDate=
             * (select Projects.DeliveryDate from Projects where Projects.Id=Trackings.Id)
             */
            Data = _context.Trackings.OrderByDescending(x=>x.SortDate)
                .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
                .Take((int)pageResults),
            CurrentPage = page,
            Pages = (int)pageCount
        };
    }

    public Task<Tracking?> GetTrackingByIdAsync(Guid id)
    {
        return _context.Trackings.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    //搜索针对Tracking
    public async Task<PaginationResult<IQueryable<Tracking>>> SearchTrackingsAsync(string searchText,int page)
    {
        var pageResults = 10f;//默认一页显示数据条数
        var pageCount = Math.Ceiling(SearchProjects(searchText).Count() / pageResults);//计算页总数
        //查询其实是project的内容，但只获取id
        var ids1=await SearchProjects(searchText)
            .Select(x => x.Id).ToListAsync();
        var ids2 = await SearchProblems(searchText)
            .Select(x => x.ProjectId).ToListAsync();
        var ids3=await SearchIssues(searchText)
            .Select(x=>x.ProjectId).ToListAsync();
        var ids= ids1.Union(ids2).Union(ids3).ToList();

        var trackings= _context.Trackings.Where(x=>ids.Contains(x.Id))
            .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
            .Take((int)pageResults);
        return new PaginationResult<IQueryable<Tracking>>
        {
            Data = trackings,
            CurrentPage = page,
            Pages = (int)pageCount
        };
    }

    private IQueryable<Problem> SearchProblems(string serchText)
    {
        return _context.Problems
            .Where(x => x.Description.ToLower().Contains(serchText.ToLower()) 
                        || x.Solution.ToLower().Contains(serchText.ToLower()));
    }
    private IQueryable<Issue> SearchIssues(string serchText)
    {
        return _context.Issues
            .Where(x => x.Description.ToLower().Contains(serchText.ToLower()));
    }
    private IQueryable<Project> SearchProjects(string searchText)
    {
        return _context.Projects
            .Where(x => x.OdpNumber.ToLower().Contains(searchText.ToLower())
                        || x.Name.ToLower().Contains(searchText.ToLower())
                        || x.SpecialNotes.ToLower().Contains(searchText.ToLower()));
    }

    public Task<List<string>> GetProjectSearchSuggestions(string searchText)
    {
        List<string> result = new List<string>();
        var projects = SearchProjects(searchText);
        foreach (var project in projects)
        {
            if (project.OdpNumber.ToLower().Contains(searchText.ToLower()))
            {
                result.Add(project.OdpNumber);
            }
            if (project.Name.ToLower().Contains(searchText.ToLower()))
            {
                result.Add(project.Name);
            }
            GetSuggestText(searchText, project.SpecialNotes, ref result);
        }
        var problems = SearchProblems(searchText);
        foreach (var problem in problems)
        {
            GetSuggestText(searchText, problem.Description, ref result);
            GetSuggestText(searchText, problem.Solution, ref result);
        }
        var issues = SearchIssues(searchText);
        foreach (var issue in issues)
        {
            GetSuggestText(searchText, issue.Description,ref result);
        }
        return Task.FromResult(result);
    }

    private void GetSuggestText(string searchText, string? str,ref List<string> result )
    {
        if (str != null)
        {
            var punctuation = str.Where(char.IsPunctuation)
                .Distinct().ToArray(); //punctuation是标点符号
            var words = str.Split()
                .Select(s => s.Trim(punctuation));
            foreach (var word in words)
            {
                if (word.ToLower().Contains(searchText.ToLower()) && !result.Contains(word))
                {
                    result.Add(word);
                }
            }
        }
    }

    #endregion
    
    #region Problem
    public Task<IQueryable<Problem>> GetProblemsAsync()
    {
        return Task.FromResult(_context.Problems.AsQueryable());
    }
    public Task<IQueryable<Problem>> GetProblemsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Problems.Where(x => x.ProjectId.Equals(projectId)).AsQueryable());
    }
    public Task<Problem?> GetProblemByIdAsync(Guid id)
    {
        return _context.Problems.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<Problem>> GetNotResolvedProblemsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Problems.Where(x => x.ProjectId.Equals(projectId)&&!x.IsClosed).AsQueryable());
    }
    #endregion

    #region Issue
    public Task<IQueryable<Issue>> GetIssuesAsync()
    {
        return Task.FromResult(_context.Issues.AsQueryable());
    }

    public Task<IQueryable<Issue>> GetIssuesByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Issues.Where(x => x.ProjectId.Equals(projectId)).AsQueryable());
    }

    public Task<Issue?> GetIssueByIdAsync(Guid id)
    {
        return _context.Issues.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    #endregion
}