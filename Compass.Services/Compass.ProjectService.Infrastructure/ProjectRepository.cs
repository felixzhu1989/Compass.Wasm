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
    public Task<IQueryable<Project>> GetProjectsAsync()
    {
        return Task.FromResult(_context.Projects.AsQueryable());

        //分页基本逻辑
        //var pageResults = 15f;//默认一页显示数据条数
        //var pageCount = Math.Ceiling(_context.Projects.Count() / pageResults);//计算页总数
        //return Task.FromResult(new PaginationResult<IQueryable<Project>>
        //{
        //    Result = _context.Projects
        //        .OrderByDescending(x => x.DeliveryDate)
        //        .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
        //        .Take((int)pageResults),
        //    CurrentPage = page,
        //    Pages = (int)pageCount
        //});
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
    
    public Task<IQueryable<Project>> GetUnbindProjectsAsync(List<Guid?> ids)
    {
       return Task.FromResult(_context.Projects.Where(x => !ids.Contains(x.Id)));
    }

    #endregion

    #region Drawing
    public Task<IQueryable<Drawing>> GetDrawingsAsync()
    {
        return Task.FromResult(_context.Drawings.AsQueryable());
    }
    public Task<Drawing?> GetDrawingByIdAsync(Guid id)
    {
        return _context.Drawings.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    //扩展查询
    public Task<IQueryable<Drawing>> GetDrawingsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Drawings.Where(x => x.ProjectId.Equals(projectId)).OrderBy(x => x.Batch).ThenBy(x=>x.ItemNumber).AsQueryable());
    }


    public Task<IQueryable<Drawing>> GetDrawingsByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    #endregion

    #region Module
    public Task<IQueryable<Module>> GetModulesAsync()
    {
        return Task.FromResult(_context.Modules.AsQueryable());
    }
    public Task<Module?> GetModuleByIdAsync(Guid id)
    {
        return _context.Modules.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }


    public Task<IQueryable<Module>> GetModulesByDrawingIdAsync(Guid drawingId)
    {
        return Task.FromResult(_context.Modules.Where(x => x.DrawingId.Equals(drawingId)).OrderBy(x => x.Name).AsQueryable());
    }

    public Task<bool> ModuleExistsInDrawing(Guid drawingId)
    {
        return _context.Modules.AnyAsync(x => x.DrawingId.Equals(drawingId));
    }

    public async Task<string?> GetDrawingUrlByModuleIdAsync(Guid id)
    {
        var module =await GetModuleByIdAsync(id);
        var drawing = await GetDrawingByIdAsync(module.DrawingId);
        return drawing.DrawingUrl;
    }



    #endregion

    #region CutList
    public Task<IQueryable<CutList>> GetCutListsAsync()
    {
        return Task.FromResult(_context.CutLists.AsQueryable());
    }

    public Task<CutList?> GetCutListByIdAsync(Guid id)
    {
        return _context.CutLists.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<CutList>> GetCutListsByModuleIdAsync(Guid moduleId)
    {
        //先按照零件排序，然后按照材料倒序排序，最后按照厚度倒序排序
        return Task.FromResult(_context.CutLists.Where(x => x.ModuleId.Equals(moduleId)).OrderBy(x => x.PartNo).ThenByDescending(x=>x.Material).ThenByDescending(x=>x.Thickness).AsQueryable());
    }

    #endregion
    
    #region DrawingPlan

    public Task<ApiPaginationResponse<IQueryable<DrawingPlan>>> GetDrawingPlansAsync(int page)
    {
        var pageResults = 5f;//默认一页显示数据条数
        var pageCount = Math.Ceiling(_context.DrawingsPlan.Count() / pageResults);//计算页总数
        return Task.FromResult(new ApiPaginationResponse<IQueryable<DrawingPlan>>
        {
            //Result = _context.DrawingsPlan
            //    .OrderByDescending(x => x.ReleaseTime)
            //    .Skip((page - 1) * (int)pageResults)//page为当前页，因此跳过前几页
            //    .Take((int)pageResults),
                
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

    //public async Task<bool> IsDrawingsNotAssignedByProjectIdAsync(Guid projectId)
    //{
    //    var drawings = await GetDrawingsByProjectIdAsync(projectId);
    //    return drawings.Any(x => x.UserId == null || x.UserId.Equals(Guid.Empty));
    //}

    public async Task<IEnumerable<Drawing>> GetDrawingsNotAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        var notAssignedDrawings = new List<Drawing>();
        //foreach (var drawing in drawings)
        //{
        //    if (drawing.UserId == null)//空的
        //    {
        //        notAssignedDrawings.Add(drawing);
        //    }
        //    else if (drawing.UserId.Equals(Guid.Empty))//全部是0
        //    {
        //        notAssignedDrawings.Add(drawing);
        //    }
        //}
        return notAssignedDrawings;
    }

    public async Task<Dictionary<Guid, IQueryable<Drawing>>> GetDrawingsAssignedByProjectIdAsync(Guid projectId)
    {
        var drawings = await GetDrawingsByProjectIdAsync(projectId);
        //var userIds = drawings.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.UserId).Distinct();
        var assignedDrawings = new Dictionary<Guid, IQueryable<Drawing>>();
        //if (userIds != null )
        //{
        //    //userIds.OfType<Guid>()
        //    foreach (var userId in userIds)
        //    {
        //        var items = drawings.Where(x => x.UserId.Equals(userId));
        //        assignedDrawings.Add(userId.GetValueOrDefault(), items);
        //    }
        //}
        return assignedDrawings;
    }

    public async Task AssignDrawingsToUserAsync(IEnumerable<Guid> drawingIds, Guid userId)
    {
        foreach (var drawingId in drawingIds)
        {
            var dbDrawing = await GetDrawingByIdAsync(drawingId);
        }
    }

    #endregion
    
    #region Search
    private IQueryable<Problem> SearchProblems(string serchText)
    {
        return _context.Problems
            .Where(x => x.Description.ToLower().Contains(serchText.ToLower())
                        || x.Solution.ToLower().Contains(serchText.ToLower()));
    }
    private IQueryable<Lesson> SearchIssues(string serchText)
    {
        return _context.Lessons
            .Where(x => x.Content.ToLower().Contains(serchText.ToLower()));
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
            GetSuggestText(searchText, issue.Content, ref result);
        }
        return Task.FromResult(result);
    }

    private void GetSuggestText(string searchText, string? str, ref List<string> result)
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
    
    #region Lesson
    //基本
    public Task<IQueryable<Lesson>> GetLessonsAsync()
    {
        return Task.FromResult(_context.Lessons.AsQueryable());
    }
    public Task<Lesson?> GetLessonByIdAsync(Guid id)
    {
        return _context.Lessons.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
    //扩展
    public Task<IQueryable<Lesson>> GetLessonsByProjectIdAsync(Guid projectId)
    {
        return Task.FromResult(_context.Lessons.Where(x => x.ProjectId.Equals(projectId)).AsQueryable());
    }
    #endregion
}