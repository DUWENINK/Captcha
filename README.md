
# 项目基于 [Hei.Captcha](https://github.com/gebiWangshushu/Hei.Captcha) 为 [Hei.Captcha](https://github.com/gebiWangshushu/Hei.Captcha) 的依赖注入版本

# 示例

## 中文泡泡验证码



![img](images/BubbleCode.png)

![1564563919705](images/1564563919705.png)

![1564563740706](images/1564563740706.png)





## 字母数字组合验证码

![img](images/HybridCode.png)

![1564563801717](images/1564563801717.png)

![1564563816361](images/1564563816361.png)

![1564563853298](images/1564563853298.png)

![1564563877047](images/1564563877047.png)

## 表单Demo

![1564564569409](images/1564564569409.png)



# 如何使用
**添加包**

```
Install-Package DUWENINK.Captcha
```
**添加注入(在 StartUp.cs 文件的 [public void ConfigureServices(IServiceCollection services)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.conventionbasedstartup.configureservices?view=aspnetcore-2.2) 方法中)**

```
        public void ConfigureServices(IServiceCollection services)
        {
              services.AddControllersWithViews(); // 对于MVC项目
              services.AddMemoryCache();//使用缓存 
              services.AddDUWENINKCaptcha();//使用验证码
              services.AddControllersWithViews();
        }

```
**在Controller中添加注入**

```
        /// <summary>
        /// 依赖注入
        /// </summary>
        private SecurityCodeHelper _securityCode ;

        private readonly IMemoryCache _cache;
        public HomeController(SecurityCodeHelper securityCode, IMemoryCache cache)
        {
            _securityCode = securityCode;
            _cache = cache;
        }

```
```
#region 生成验证码
        /// <summary>
        /// 泡泡中文验证码 
        /// </summary>
        /// <returns></returns>
        [HttpPost("BubbleCode")]
        public IActionResult BubbleCode()
        {
            var code = _securityCode.GetRandomCnText(2);//生成的中文验证码
            var vGuid = Guid.NewGuid().ToString();//guid
            var imgbyte = _securityCode.GetBubbleCodeByte(code);//生成的中文图片
            //相对于现在的过期时间 缓存相对于现在的时间后5分钟失效
            _cache.Set(vGuid, code, new TimeSpan(0, 1, 0));
            Response.Cookies.Append("validatecode", vGuid);//把生成的唯一Guid添加到cookie发送给前端
            return File(imgbyte, "image/png");
        }

        /// <summary>
        /// 数字字母组合验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost("HybridCode")]
        public IActionResult HybridCode()
        {
            var code = _securityCode.GetRandomEnDigitalText(4);
            var vGuid = Guid.NewGuid().ToString();//guid
            var imgbyte = _securityCode.GetEnDigitalCodeByte(code);
            //相对于现在的过期时间 缓存相对于现在的时间后5分钟失效
            _cache.Set(vGuid, code, new TimeSpan(0, 1, 0));
            Response.Cookies.Append("validatecode", vGuid);//把生成的唯一Guid添加到cookie发送给前端
            return File(imgbyte, "image/png");
        }

        #endregion

        #region 验证验证码
        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <returns></returns>
        [HttpPost("VerifyCode")]
        public NormalResult<bool> VerifyCode([FromBody]ValidatecodeDto filter)
        {
            _cache.TryGetValue(filter.ValidatecodeFromCookie, out string value);
            return new NormalResult<bool> { Data = string.Equals(value, filter.TextByUser, StringComparison.OrdinalIgnoreCase) };
        }

        #endregion
 ```



### 高级

参照Demo， 通过修改/丰富应用程序运行目录`./fonts`目录下的字体文件，生成更多不同字体组合的验证码。
