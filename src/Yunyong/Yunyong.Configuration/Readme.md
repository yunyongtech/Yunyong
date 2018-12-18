# 使用实例
添加配置：

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("AppSettings.json");
                    config.AddJsonFile("EventBusConfig.json");
                    ......

                    // JsonSection
                    config.Add(new JsonSectionConfigurationSource()
                    {
                        ["CustomConfig"] = new MediaConfig()
                        {
                            AccessKey = "XXX",
                            SecretKey = "XXX",
                            Bucket = "XXX",
                            CloudUrlPrefix = "http://www.xxx.com/"
                        }
                    });

                    var jsonString = "{'extkey':{'id':123,'x':'abc'}}";
                    //JsonString
                    config.Add(new JsonStringConfigurationSource(jsonString));
                })
                .UseStartup<Startup>();
        }

获取配置：

        public static IServiceCollection RegisterServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.RegisterEasyNetQ(configuration.Get<EventBusConfig>("EventBusConfig"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ......

            // media config
            var mediaConfig = configuration.Get<MediaConfig>("QiniuConfig");

            // ext config
            var ext = configuration.Get<ExtConfig>("extkey");

            return services;
        }