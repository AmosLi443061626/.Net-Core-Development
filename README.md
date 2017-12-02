# <p><a href="http://shang.qq.com/wpa/qunwpa?idkey=d1df0014a95f198b397bebba8c2c6b30012fe8aae39dfc58b37637a6a67e439d">点击加入群【.NET大型网站架构】QQ群:433685124</a></p>


# 注意事项:
## 不允许Join查询，可以使用多查的方式  >> 分表分库
### 配置获取统一用 ConfigManagerConf.Get("**")    >>  扩展 etcd 动态更新
#### 所有方法调用使用 IocContainer.Container.Get<??> Ioc模式  >> AOP模式特性处理方法
##### 日志记录使用 Log 方法   >>  扩展集中日志发送 
###### 初始化请看 Program.cs 文件

### ORM请移至: https://github.com/yuzd/AntData.ORM 


###### 项目简介： <br/>
01_BusinessService:业务层（接口模式） <br/> 
IStandard.Examples (接口层) <br/>  
Standard.Examples (实现层)   <br/>
02_DB(DB层) <br/>
+DBContextEntity:qlServer/Mysql 模型生成层及默认DB初始化层  <br/>
+mysql文件夹下：EntityRobotForMysql.tt 保存(Ctrl+S)即生成数据库模型;EntityRobotForMysql.tt中LoadMySqlMetadata <br/> 
("连接字符串") 配置生成模型的数据库连接字符串  <br/>
03_Domain(Model层) <br/>
+Domains:业务model  <br/>
04_Infrastructure(基础服务) <br/> 
+CacheOperation:缓存处理  <br/>
+Checks:验证扩展  <br/>
+Configs:配置信息管理(使用Etcd) <br/>
+Exceptions:队列异常 <br/>
+Extensions:帮助层,内含<Json/HttpRequest/GUID/时间> 等扩展 <br/>
+Ioc(控制反转):控制反转全局组件  <br/>
+BaseAspectAttribute:基础属性  <br/>
+CacheAttribute:缓存属性  <br/>
+LoggerAttribute:日志属性  <br/>
+TopSubscribeAttribute:订阅属性 <br/>
+IocContainer.RegisterAssembly（实现,接口）:注册组件 <br/>
+IocContainer.Container.Build()更新注册信息 <br/>
+IonCOntainer.Get<T>()配置组件 <br/>
+Logs:日志     <br/>
+Mappering:DTO数据转换 <br/>
+MessageMQ:消息队列  <br/>
+Pools:池化扩展   <br/>
+RedisHelper:redis操作  <br/>
+Result:统一结果返回类   <br/>
+RequestExtend:Htpp请求扩展  <br/>
NLog.config --日志配置   <br/>
05_startups(服务启动层)  <br/>
+startup(示列)：  <br/>
##### 所有的程序统一使用Ioc管理,要是程序生效，必须先到Program中注册

<br/><br/><br/>
###### 第一步：<br/> 

使用ORM工具生成数据库模型:  <br/>
     DB -> EntityRobotForMysql.tt 中 LoadMySqlMetadata("连接字符串") 配置生成模型的数据库连接字符串,然后保存生成数据模型 <br/>

使用ORM 不能使用Join关联查询;<br/>

ORM 确定不能满足的情况下,请在DBContextEntity 项目中新建类,进行T-SQL编写,编写后调用;  <br/> 

ORM:使用方式请查看 <br/>
https://github.com/yuzd/AntData.ORM <br/>

    
###### 第二步：<br/>
修改数据库连接字符串：<br/>
    4_Infrastructure(基础服务)->Configs->ConfigManagerConf <br/>

###### 第三步:  \<br>
编写业务及开放业务接口(方法统一返回参数:Result): <br/>
	01_BusinesServices(业务层) -> IBusiness   (开发接口)  统一返回参数:Result   <br/>
	    01_BusinesServices(业务层) -> Business (实现业务) <br/>


###### Docker + k8s + .Net Core + Etcd  -> Pass
