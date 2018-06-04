/*******************************************************************************
* Copyright (C) JuCheap.Com
* 
* Author: dj.wong
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@JuCheap.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;

namespace JuCheap.Data
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    internal sealed class DbConfiguration : DbMigrationsConfiguration<JuCheapContext>
    {
        private readonly DateTime _now = new DateTime(2015, 5, 1, 23, 59, 59);
        private readonly BaseIdGenerator _instance = BaseIdGenerator.Instance;

        public DbConfiguration()
        {
            AutomaticMigrationsEnabled = true;//启用自动迁移
            AutomaticMigrationDataLossAllowed = true;//是否允许接受数据丢失的情况，false=不允许，会抛异常；true=允许，有可能数据会丢失
        }

        protected override void Seed(JuCheapContext context)
        {
            if (!context.Set<SystemConfigEntity>().Any(item => item.IsDataInited))
            {
                #region 菜单

                var system = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "系统设置",
                    Icon = "fa fa-gear",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 1,
                    Code = "AA",
                    PathCode = "AA",
                    Type = 1
                }; //1
                var menuMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "菜单管理",
                    Url = "/Menu/Index",
                    CreateDateTime = _now,
                    Order = 2,
                    Code = "AA",
                    PathCode = "AAAA",
                    Type = 2
                }; //2
                var departmentMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "部门管理",
                    Url = "/Department/Index",
                    CreateDateTime = _now,
                    Order = 3,
                    Code = "AG",
                    PathCode = "AAAG",
                    Type = 2
                }; //3
                var roleMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "角色管理",
                    Url = "/Role/Index",
                    CreateDateTime = _now,
                    Order = 3,
                    Code = "AB",
                    PathCode = "AAAB",
                    Type = 2
                }; //3
                var userMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "用户管理",
                    Url = "/User/Index",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AC",
                    PathCode = "AAAC",
                    Type = 2
                }; //4
                var userRoleMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userMgr.Id,
                    Name = "用户授权",
                    Url = "/User/Authen",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AD",
                    PathCode = "AAAD",
                    Type = 2
                }; //5
                var roleMenuMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "角色授权",
                    Url = "/Role/Authen",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AE",
                    PathCode = "AAAE",
                    Type = 2
                }; //6
                var sysConfig = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "系统配置",
                    Url = "/System/Index",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AF",
                    PathCode = "AAAF",
                    Type = 2
                }; //7
                var areaConfig = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "省市区管理",
                    Url = "/Area/Index",
                    CreateDateTime = _now,
                    Order = 5,
                    Code = "AH",
                    PathCode = "AAAH",
                    Type = 2
                };
                var sysConfigReloadPathCode = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = sysConfig.Id,
                    Name = "重置路径码",
                    Url = "/System/ReloadPathCode",
                    CreateDateTime = _now,
                    Order = 1,
                    Code = "AA",
                    PathCode = "AAAFAA",
                    Type = 3
                }; //8
                var sysConfigReloadTasks = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = sysConfig.Id,
                    Name = "重新加载Hangfire定时任务",
                    Url = "/System/ReloadTasks",
                    CreateDateTime = _now,
                    Order = 2,
                    Code = "AB",
                    PathCode = "AAAFAB",
                    Type = 3
                }; //8
                var log = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "日志查看",
                    Icon = "fa fa-bars",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 2,
                    Code = "AB",
                    PathCode = "AB",
                    Type = 1
                }; //9
                var logLogin = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "登录日志",
                    Url = "/Log/Logins",
                    CreateDateTime = _now,
                    Order = 9,
                    Code = "AA",
                    PathCode = "ABAA",
                    Type = 2
                }; //10
                var logView = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "访问日志",
                    Url = "/Log/Visits",
                    CreateDateTime = _now,
                    Order = 10,
                    Code = "AB",
                    PathCode = "ABAB",
                    Type = 2
                }; //11

                var logChart = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "图表统计",
                    Url = "/Log/Charts",
                    CreateDateTime = _now,
                    Order = 11,
                    Code = "AC",
                    PathCode = "ABAC",
                    Type = 2
                };
                var userRoleSet = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userRoleMgr.Id,
                    Name = "用户角色授权",
                    Url = "/User/GiveRight",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AA",
                    PathCode = "AAADAA",
                    Type = 3
                };
                var userRoleCancel = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userRoleMgr.Id,
                    Name = "用户角色取消",
                    Url = "/User/CancelRight",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AB",
                    PathCode = "AAADAB",
                    Type = 3
                };
                //菜单
                var menus = new List<MenuEntity>
                {
                    system,
                    menuMgr,
                    departmentMgr,
                    roleMgr,
                    userMgr,
                    userRoleMgr,
                    userRoleSet,
                    userRoleCancel,
                    roleMenuMgr,
                    sysConfig,
                    areaConfig,
                    sysConfigReloadPathCode,
                    sysConfigReloadTasks,
                    log,
                    logLogin,
                    logView,
                    logChart
                };
                var menuBtns = GetMenuButtons(menuMgr.Id, "Menu", "菜单", "AAAA"); //14
                var rolwBtns = GetMenuButtons(roleMgr.Id, "Role", "角色", "AAAB"); //17
                var userBtns = GetMenuButtons(userMgr.Id, "User", "用户", "AAAC"); //20
                var departmentBtns = GetMenuButtons(departmentMgr.Id, "Department", "部门", "AAAG");
                var areaBtns = GetMenuButtons(departmentMgr.Id, "Area", "省市区", "AAAH");

                menus.AddRange(menuBtns);
                menus.AddRange(rolwBtns);
                menus.AddRange(userBtns);
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userMgr.Id,
                    Name = "重置密码",
                    Url = "/User/ResetPwd",
                    CreateDateTime = _now,
                    Order = 4,
                    Code = "AD",
                    PathCode = "AAACAD",
                    Type = 3
                });
                menus.AddRange(departmentBtns);
                menus.AddRange(areaBtns);
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = roleMenuMgr.Id,
                    Order = 6,
                    Name = "授权",
                    Type = 3,
                    Url = "/Role/SetRoleMenus",
                    CreateDateTime = _now,
                    Code = "AA",
                    PathCode = "AAACAA"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = roleMenuMgr.Id,
                    Order = 6,
                    Name = "清空权限",
                    Type = 3,
                    Url = "/Role/ClearRoleMenus",
                    CreateDateTime = _now,
                    Code = "AB",
                    PathCode = "AAACAB"
                });

                //示例页面
                var pages = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "示例页面",
                    Icon = "fa fa-file-o",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 3,
                    Code = "AC",
                    PathCode = "AC",
                    Type = 1
                };
                menus.Add(pages);
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 0,
                    Name = "按钮",
                    Type = 2,
                    Url = "/Pages/Buttons",
                    CreateDateTime = _now,
                    Code = "AA",
                    PathCode = "ACAA"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 1,
                    Name = "表单",
                    Type = 2,
                    Url = "/Pages/Form",
                    CreateDateTime = _now,
                    Code = "AB",
                    PathCode = "ACAB"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 2,
                    Name = "高级表单",
                    Type = 2,
                    Url = "/Pages/FormAdvance",
                    CreateDateTime = _now,
                    Code = "AC",
                    PathCode = "ACAC"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 3,
                    Name = "表格",
                    Type = 2,
                    Url = "/Pages/Tables",
                    CreateDateTime = _now,
                    Code = "AD",
                    PathCode = "ACAD"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 4,
                    Name = "选项卡",
                    Type = 2,
                    Url = "/Pages/Tabs",
                    CreateDateTime = _now,
                    Code = "AE",
                    PathCode = "ACAE"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = pages.Id,
                    Order = 5,
                    Name = "字体FontAwesome",
                    Type = 2,
                    Url = "/Pages/FontAwesome",
                    CreateDateTime = _now,
                    Code = "AF",
                    PathCode = "ACAF"
                });

                #endregion

                #region 角色

                var superAdminRole = new RoleEntity
                {
                    Id = _instance.GetId(),
                    Name = "超级管理员",
                    Description = "超级管理员",
                    Menus = menus
                };
                var guestRole = new RoleEntity
                {
                    Id = _instance.GetId(),
                    Name = "guest",
                    Description = "游客",
                    Menus = menus.Where(x => x.Type != 3).ToList()
                };
                var roles = new List<RoleEntity>
                {
                    superAdminRole,
                    guestRole
                };

                #endregion

                #region 用户

                var admin = new UserEntity
                {
                    Id = _instance.GetId(),
                    LoginName = "admin",
                    RealName = "超级管理员",
                    Password = "123321".ToMd5(),
                    Email = "szmacio@163.com",
                    IsSuperMan = true,
                    CreateDateTime = _now,
                    Roles = new List<RoleEntity> { superAdminRole }
                };
                var guest = new UserEntity
                {
                    Id = _instance.GetId(),
                    LoginName = "admin",
                    RealName = "游客",
                    Password = "qwaszx".ToMd5(),
                    Email = "service@jucheap.com",
                    CreateDateTime = _now,
                    Roles = new List<RoleEntity> { guestRole }
                };
                //用户
                var user = new List<UserEntity>
                {
                    admin,
                    guest
                };

                #endregion

                #region 系统配置

                var systemConfig = new List<SystemConfigEntity>
                {
                    new SystemConfigEntity
                    {
                        Id = _instance.GetId(),
                        SystemName = "JuCheap",
                        IsDataInited = true,
                        DataInitedDate = _now,
                        CreateDateTime = _now,
                        IsDeleted = false
                    }
                };

                #endregion

                #region 路径码

                var pathCodes = InitData.GetPathCodes();

                #endregion

                AddOrUpdate(context, m => m.LoginName, user.ToArray());
                AddOrUpdate(context, m => new {m.ParentId, m.Name}, menus.ToArray());
                AddOrUpdate(context, m => m.Name, roles.ToArray());
                AddOrUpdate(context, m => m.SystemName, systemConfig.ToArray());
                AddOrUpdate(context, m => m.Code, pathCodes.ToArray());
            }
        }

        #region Private

        /// <summary>
        /// 获取菜单的基础按钮
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="controllerShowName">菜单显示名称</param>
        /// <param name="parentPathCode">父级路径码</param>
        /// <returns></returns>
        private IEnumerable<MenuEntity> GetMenuButtons(string parentId, string controllerName,string controllerShowName,string parentPathCode)
        {
            return new List<MenuEntity>
            {
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("添加",controllerShowName),
                    Url = string.Format("/{0}/Add",controllerName),
                    CreateDateTime = _now,
                    Order = 1,
                    Code = "AA",
                    PathCode = parentPathCode+"AA",
                    Type = 3
                },
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("修改",controllerShowName),
                    Url = string.Format("/{0}/Edit",controllerName),
                    CreateDateTime = _now,
                    Order = 2,
                    Code = "AB",
                    PathCode = parentPathCode+"AB",
                    Type = 3
                },
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("删除",controllerShowName),
                    Url = string.Format("/{0}/Delete",controllerName),
                    CreateDateTime = _now,
                    Order = 3,
                    Code = "AC",
                    PathCode = parentPathCode+"AC",
                    Type = 3
                }
            };
        }

        /// <summary>
        /// 添加更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="exp"></param>
        /// <param name="param"></param>
        void AddOrUpdate<T>(DbContext context, Expression<Func<T, object>> exp, T[] param) where T : class
        {
            var set = context.Set<T>();
            set.AddOrUpdate(exp, param);
        }

        #endregion
    }
}
