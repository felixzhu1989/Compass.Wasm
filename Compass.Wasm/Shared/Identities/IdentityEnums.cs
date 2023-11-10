namespace Compass.Wasm.Shared.Identities;

public enum Roles_e
{
    admin, //管理员
    mgr, //Manager管理层
    pm, //Project Manager项目经理
    pmc, //Production And Material Control生产及物料控制
    pur, //Purchase采购
    dsr, //Designer工程师
    nest, //Nesting排版
    prod, //Production生产
    cut,//Laser/punch breaker cutting 激光切割/冲床切割
    //Bending 折弯
    //Cutting film & parts collection 切膜&配料
    //Welding Assembly 焊接
    //Polishing 抛光
    //Hood Assembly 结构装配
    //Ansul pre-piping Ansul预埋管装配
    //Water Washing piping 水洗管装配
    //Electrical Assembly 电气装配
    //Assembly Inspection组装检验
    insp, //Inspector巡检
    qc, //Quality Control质量
    //Packaging 打包
    whse,//Warehouse仓库
}