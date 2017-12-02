 ## <p><a href="http://shang.qq.com/wpa/qunwpa?idkey=d1df0014a95f198b397bebba8c2c6b30012fe8aae39dfc58b37637a6a67e439d">点击加入群【.NET大型网站架构】QQ群:433685124</a></p>


# 注意事项:
## 不允许Join查询，可以使用多查的方式  >> 分表分库
### 配置获取统一用 ConfigManagerConf.Get("**")    >>  扩展 etcd 动态更新
#### 所有方法调用使用 IocContainer.Container.Get<??> Ioc模式  >> AOP模式特性处理方法
##### 日志记录使用 Log 方法   >>  扩展集中日志发送 
###### 初始化请看 Program.cs 文件

### ORM请移至: https://github.com/yuzd/AntData.ORM 

#### 项目简介： <br/>
###### BusinessService:业务层（接口模式）
<ul>
	<li>IStandard.Examples (接口层)</li>
	<li>Standard.Examples  (实现层)</li>
</ul>

##### DB(DB层)
<ul>
	<li>DBContextEntity:qlServer/Mysql 模型生成层及默认DB初始化层
	<br/> +mysql文件夹下：EntityRobotForMysql.tt 保存(Ctrl+S)即生成数据库模型;EntityRobotForMysql.tt中LoadMySqlMetadata <br/> 
("连接字符串") 配置生成模型的数据库连接字符串 	
	</li>
</ul>

##### Domain(Model层) 
<ul>
	<li>Domains:业务model</li>
</ul>

###### Infrastructure(基础服务) 
<ul>
	<li>CacheOperation:缓存处理</li>
	<li>Checks:验证扩展</li>
	<li>Configs:配置信息管理(使用Etcd)</li>
	<li>Exceptions:异常</li>
	<li>Extensions:帮助层,内含<Json/HttpRequest/GUID/时间> 等扩展</li>
	<li>Ioc(控制反转):控制反转全局组件</li>
	<li>BaseAspectAttribute:基础属性</li>
	<li>CacheAttribute:缓存属性 </li>
	<li>LoggerAttribute:日志属性</li>
	<li>TopSubscribeAttribute:订阅属性</li>
	<li>IocContainer.RegisterAssembly（实现,接口）:注册组件 <br/>IocContainer.Container.Build()更新注册信息 <br/>IonCOntainer.Get<T>()配置组件</li>
	<li>Logs:日志  >>  NLog.config</li>
	<li>Mappering:DTO数据转换</li>
	<li>MessageMQ:消息队列 (Rabbitmq)</li>
	<li>Pools:池化扩展</li>
	<li>RedisHelper:redis操作</li>
	<li>Pools:池化扩展</li>
	<li>Result:统一结果返回类</li>
	<li>RequestExtend:Htpp请求扩展</li>
</ul>

###### startups(服务启动层)
<ul>
	<li>startup(示列)</li>
	<li>Filters(拦截)<br/>
	    LogFilterAttribute:日志拦截
	</li>
</ul>

##### 所有的程序统一使用Ioc管理,要是程序生效，必须先到Program中注册
<br/>

###### 第一步： &nbsp;&nbsp;&nbsp;&nbsp;使用ORM工具生成数据库模型
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;DB -> EntityRobotForMysql.tt 中 LoadMySqlMetadata("连接字符串") 配置生成模型的数据库连接字符串,然后保存生成数据模型 <br/>
&nbsp;&nbsp;&nbsp;&nbsp;ORM 确定不能满足的情况下,请在DBContextEntity 项目中新建类,进行T-SQL编写,编写后调用;  <br/> 
&nbsp;&nbsp;&nbsp;&nbsp;ORM: 使用方式请查看 
## https://github.com/yuzd/AntData.ORM 

###### 第二步：Infrastructure(基础服务)->Configs->ConfigManagerConf 设置Mysql连接字符串<br/>

###### 第三步: 编写业务及开放业务接口(方法统一返回参数:Result): <br/>

 <br/> ==
 
###### Docker + k8s + .Net Core + Etcd  -> Pass
