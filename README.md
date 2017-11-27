*************************************
注意事项:
1.不能join查询,保存所有的join查询
2.配置获取统一用：ConfigManagerConf.Get("**") 获取
3.所有方法调用使用IocContainer.Container.Get<??> Ioc模式
4.日志记录使用Log 方法
5.初始化请看Program.cs文件
*************************************


项目分为：

01_BusinessService:业务层（接口模式）
   IStandard.Examples (接口层)
   Standard.Examples (实现层)
02_DB(DB层)
    +DBContextEntity:qlServer/Mysql 模型生成层及默认DB初始化层
	+mysql文件夹下：EntityRobotForMysql.tt 保存(Ctrl+S)即生成数据库模型;EntityRobotForMysql.tt中LoadMySqlMetadata
                        ("连接字符串") 配置生成模型的数据库连接字符串
03_Domain(Model层)
    +Domains:业务model
04_Infrastructure(基础服务)
    +CacheOperation:缓存处理
    +Checks:验证扩展
    +Configs:配置信息管理(使用Etcd)
    +Exceptions:队列异常
    +Extensions:帮助层,内含<Json/HttpRequest/GUID/时间> 等扩展
    +Ioc(控制反转):控制反转全局组件
        +BaseAspectAttribute:基础属性
	+CacheAttribute:缓存属性
	+LoggerAttribute:日志属性
	+TopSubscribeAttribute:订阅属性
            +IocContainer.RegisterAssembly（实现,接口）:注册组件
            +IocContainer.Container.Build()更新注册信息
            +IonCOntainer.Get<T>()配置组件
    +Logs:日志
    +Mappering:DTO数据转换
    +MessageMQ:消息队列
    +Pools:池化扩展
    +RedisHelper:redis操作
    +Result:统一结果返回类、
    +RequestExtend:Htpp请求扩展
    NLog.config --日志配置
05_startups(服务启动层)
    +startup(示列)：


所有的程序统一使用Ioc管理,要是程序生效，必须先到Program中注册


————————————————————————————————————
第一步：

使用ORM工具生成数据库模型:
     DB -> EntityRobotForMysql.tt 中 LoadMySqlMetadata("连接字符串") 配置生成模型的数据库连接字符串,然后保存生成数据模型
<***************************
使用ORM 不能使用Join关联查询;

ORM 确定不能满足的情况下,请在DBContextEntity 项目中新建类,进行T-SQL编写,编写后调用;

ORM:使用方式请查看
https://github.com/yuzd/AntData.ORM
****************************>
    
第二步：
修改数据库连接字符串：
    4_Infrastructure(基础服务)->Configs->ConfigManagerConf

第三步:
编写业务及开放业务接口(方法统一返回参数:Result):
	01_BusinesServices(业务层) -> IBusiness   (开发接口)  统一返回参数:Result
	    01_BusinesServices(业务层) -> Business (实现业务)

===================
已经接入swagger UI
		
Dockerfile文件
===================