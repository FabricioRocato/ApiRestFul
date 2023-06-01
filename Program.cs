using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Umfg.Programacaoiv2023.Primeira.Api
{
    public class Program
    {
        private static List<Cliente> _lista = new List<Cliente>()
        {
            new Cliente("Teste Um"),
            new Cliente("Teste Dois"),
        };

        private static List<Produto> produtoList = new List<Produto>()
        {
            new Produto("Muito Caroo", "123456789" , "5.000"),
            new Produto("Kalamazoo", "987654321" , "10.000")
        };

        public static void Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapGet("api/v1/cliente", ObterTodosClientesAsync);
            app.MapPost("api/v1/cliente", CadastrarClienteAsync);
            app.MapPut("api/v1/cliente/{id}", AtualizarClienteAsync);
            app.MapDelete("api/v1/cliente", RemoverTodosClientesAsync);           
            
            app.MapGet("api/v1/produtp", ObterTodosProdutosAsync);
            app.MapPost("api/v1/produto", CadastrarProdutoAsync);
            app.MapPut("api/v1/produtp/{id}", AtualizarProdutoAsync);
            app.MapDelete("api/v1/produto", RemoverTodosProdutosAsync);

            app.Run();
        }

        public static async Task ObterTodosClientesAsync(HttpContext context)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(_lista);
        }

      

        public static async Task CadastrarClienteAsync(HttpContext context)
        {
            var cliente = await context.Request.ReadFromJsonAsync<Cliente>();

            if (cliente == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Não foi possivel cadastrar o cliente! Verifique.");
                return;
            }

            _lista.Add(cliente);

            context.Response.StatusCode = (int)HttpStatusCode.Created;
            await context.Response.WriteAsJsonAsync(cliente);
        }

        public static async Task AtualizarClienteAsync(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode= (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parâmetro id não foi enviado! Verifique.");

                return;
            }

            var cliente = _lista.FirstOrDefault(x => x.Id == id);

            if (cliente == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Cliente não encontrado para o id: {id}. Verfique.");

                return;
            }

            _lista.Remove(cliente);

            cliente.Nome = (await context.Request.ReadFromJsonAsync<Cliente>()).Nome;

            _lista.Add(cliente);

            context.Response.StatusCode =(int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(cliente);
        }

        public static async Task RemoverTodosClientesAsync(HttpContext context)
        {
            _lista.Clear();

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Todos os clientes foram removidos com sucesso!");
        }


        public static async Task ObterTodosProdutosAsync(HttpContext context)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(produtoList);
        }



        public static async Task CadastrarProdutoAsync(HttpContext context)
        {
            var produto = await context.Request.ReadFromJsonAsync<Produto>();

            if (produto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Não foi possivel cadastrar o produto! Verifique.");
                return;
            }

            produtoList.Add(produto);

            context.Response.StatusCode = (int)HttpStatusCode.Created;
            await context.Response.WriteAsJsonAsync(produto);
        }

        public static async Task AtualizarProdutoAsync(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parâmetro id não foi enviado! Verifique.");

                return;
            }

            var produto = produtoList.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Produto não encontrado para o id: {id}. Verfique.");

                return;
            }

            produtoList.Remove(produto);

            produto.Descricao = (await context.Request.ReadFromJsonAsync<Produto>()).Descricao;

            produtoList.Add(produto);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(produto);
        }

        public static async Task RemoverTodosProdutosAsync(HttpContext context)
        {
            produtoList.Clear();

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Todos os produtos foram removidos com sucesso!");
        }


    }
}