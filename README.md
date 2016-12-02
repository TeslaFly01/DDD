<h3>DDD系统架构说明</h3>

>开发环境：.NET 4.0	Entity Framwork 6.0.0.0（Code First）  MVC 4.0

>开发工具：VS1012、SQL Server 2008

<h4>需提前安装的VS插件：</h4>

>1、Resharper：智能代码优化工具

>2、Web Essentials 2012：
作用：为了提高页面加载性能，特将系统中JS、CSS文件利用此工具进行压缩
用法：如：Index.cshtml页面需引用一个JS文件，则需要添加index.js和index.min.js两个文件，我们只需在index.js 中进行编辑JS代码即可，编辑完成后此插件会自动压缩JS代码到index.min.js文件中，页面引用index.min.js文件即可

<h4>框架引用组件：</h4>

>1、	MvcPager：分页组件
详细用法：http://www.webdiyer.com

>2、	lhgdialog：弹窗组件
详细用法：http://www.lhgdialog.com/api/

>3、	Unity：依赖注入容器，为解决程序的高内聚、低耦合问题
版本号：2.1.505.2
详细用法：http://www.cnblogs.com/fuchongjundream/p/3915391.html

>4、	log4net：日志记录
详细用法：http://www.cnblogs.com/jys509/p/4569874.html

>5、	BeIT Memcached：分布式缓存
简介：Memcached 是一个高性能的分布式内存 对象缓存系统，用于动态Web应用以减轻数据库负载。它通过在内存中缓存数据和对象 来减少读取数据库的次数，从而提供动态、数据库驱动网站的速度。
详细用法：http://kb.cnblogs.com/page/48194/

<h5>系统架构：</h5>
>此系统采用RESTful（Representational State Transfer）的架构风格结合领域驱动来搭建而成，2004年Eric Evans 发表Domain-Driven Design –Tackling Complexity in the Heart of Software （领域驱动设计），简称Evans DDD。领域驱动设计分为两个阶段：
以一种领域专家、设计人员、开发人员都能理解的通用语言作为相互交流的工具，在交流的过程中发现领域概念，然后将这些概念设计成一个领域模型；
    由领域模型驱动软件设计，用代码来实现该领域模型；由此可见，领域驱动设计的核心是建立正确的领域模型。

以下文章请认真参考学习，因为此系统架构原型就是参考如下资料搭建而成：
<h5>关于领域驱动（Domain-Driven-Design）理论文章：</h5>
http://www.cnblogs.com/netfocus/archive/2011/10/10/2204949.html
<h5>关于MVC+EF4.1系列的文章：</h5>
http://www.cnblogs.com/wlflovenet/archive/2011/07/23/MVCANDEF.html
<h5>此系统架构原型参考文章：</h5>
[http://weblogs.asp.net/shijuvarghese/developing-web-apps-using-asp-net-mvc-3-razor-and-ef-code-first-part-1](http://weblogs.asp.net/shijuvarghese/developing-web-apps-using-asp-net-mvc-3-razor-and-ef-code-first-part-1)<br>
[http://weblogs.asp.net/shijuvarghese/developing-web-apps-using-asp-net-mvc-3-razor-and-ef-code-first-part-2](http://weblogs.asp.net/shijuvarghese/developing-web-apps-using-asp-net-mvc-3-razor-and-ef-code-first-part-2)
