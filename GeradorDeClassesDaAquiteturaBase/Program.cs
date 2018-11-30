using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeradorDeClassesDaAquiteturaBase
{
    public class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in LerCSV())
            {
                var interfaceServico2 = interfaceServico.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var interfaceRepositorio2 = interfaceRepositorio.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeServico2 = classeServico.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeRepositorio2 = classeRepositorio.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var interfaceAplicacao2 = interfaceAplicacao.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeAplicacao2 = classeAplicacao.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeEntidade2 = classeEntidade.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeConfiguracaoEntidade2 = classeConfiguracaoEntidade.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);
                var classeController2 = classeController.Replace("{project}", item.project).Replace("{system}", item.system).Replace("{.system}", $".{item.system}").Replace("{entity}", item.entity).Replace("{.integration}", $".{item.integration}").Replace("{name}", item.name);

                CriarArquivos("C:/temp/Service/Interface/", $"I{item.name}Service", interfaceServico2);
                CriarArquivos("C:/temp/Service/", $"{item.name}Service", classeServico2);
                CriarArquivos("C:/temp/App/Interface/", $"I{item.name}AppService", interfaceAplicacao2);
                CriarArquivos("C:/temp/APP/", $"{item.name}AppService", classeAplicacao2);
                CriarArquivos("C:/temp/Repository/", $"{item.name}Repository", classeRepositorio2);
                CriarArquivos("C:/temp/Repository/Interface/", $"I{item.name}Repository", interfaceRepositorio2);
                CriarArquivos("C:/temp/Domain/Entity/", item.name, classeEntidade2);
                CriarArquivos("C:/temp/EntityConfiguration/", $"{item.name}Configuration", classeConfiguracaoEntidade2);
                CriarArquivos("C:/temp/Controller/", $"{item.name}Controller", classeController2);
            }
        }

        private static IList<Objeto> LerCSV()
        {
            IList<Objeto> objetos = new List<Objeto>();

            foreach (var item in File.ReadAllLines("c:/temp/classes.csv"))
            {
                var columns = item.Split(';');
                objetos.Add(new Objeto()
                {
                    project = columns[0],
                    integration = columns[1],
                    name = columns[2],
                    entity = columns[3],
                    system = columns[4],
                });
            }

            return objetos;
        }

        private static void CriarArquivos(string localArquivo, string nomeArquivo, string conteudo)
        {
            string path = $"{localArquivo}{nomeArquivo}.cs";

            try
            {
                if (!Directory.Exists(localArquivo)) Directory.CreateDirectory(localArquivo);

                if (File.Exists(path))
                    File.Delete(path);

                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(conteudo);
                    fs.Write(info, 0, info.Length);
                }

                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private struct Objeto
        {
            public string project { get; set; }
            public string integration { get; set; }
            public string name { get; set; }
            public string entity { get; set; }
            public string system { get; set; }
        }

        private static string interfaceServico = @"using {project}.Domain{.system}Entity;

namespace {project}.Domain.Interface{.system}Service
{
    public interface I{name}Service : IServiceBase<{entity}>
    {
    }
}";

        private static string interfaceRepositorio = @"using {project}.Domain{.system}Entity;

namespace {project}.Domain{.system}Interface.Repository
{
    public interface I{name}Repository : IRepositoryBase{system}<{entity}>
    {
    }
}";

        private static string classeServico = @"using {project}.Domain.Interface{.system}Service;
using {project}.Domain{.system}Entity;
using {project}.Domain{.system}Interface.Repository;

namespace {project}.Domain{.system}Service
{
    public class {name}Service : ServiceBase<{entity}>, I{name}Service
    {
        private readonly I{name}Repository Repository;

        public {name}Service(I{name}Repository repository) : base(repository)
        {
            Repository = repository;
        }      
    }
}";

        private static string classeRepositorio = @"using {project}.Domain{.system}Entity;
using {project}.Domain{.system}Interface.Repository;

namespace {project}.Infrastructure{.integration}Data.Repository
{
    public class {name}Repository : RepositoryBase{system}<{entity}>, I{name}Repository
    {
    }
}";

        private static string interfaceAplicacao = @"using {project}.Domain{.system}Entity;

namespace {project}.Application.Interface
{
    public interface I{name}AppService : IAppServiceBase<{entity}>
    {
    }
}";

        private static string classeAplicacao = @"using {project}.Domain.Interface{.system}Service;
using {project}.Domain{.system}Entity;

namespace {project}.Application
{
    public class {name}AppService : AppServiceBase<{entity}>, I{name}AppService
    {
        private readonly I{name}Service Service;

        public {name}AppService(I{name}Service service)
            : base(service)
        {
            Service = service;
        }
    }
}";

        private static string classeEntidade = @"namespace {project}.Domain
{
    public class {entity} : EntityBase
    {
    }
}";

        private static string classeConfiguracaoEntidade = @"using {project}.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace {project}.Infrastructure{.integration}Data.EntityConfiguration
{
    public class {entity}Configuration : IEntityTypeConfiguration<{entity}>
    {
        public void Configure(EntityTypeBuilder<{entity}> builder)
        {
            /*
            builder.Property(f => f.Nome)
                .IsRequired();
            */
        }
    }
}";

        private static string classeController = @"using {project}.Application.Interface;
using {project}.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace {project}.Presentation.API.Controllers
{
    [Route(""api/{entity}"")]
    [Authorize]
    public class {entity}Controller : ControllerBase<{entity}>
    {
        public {entity}Controller(I{entity}AppService application)
        {
            Application = application;
        }
    }
}";
    }
}