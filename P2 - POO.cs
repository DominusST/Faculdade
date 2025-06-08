using System;
using System.Collections.Generic;

namespace LojaVirtual
{
    // Entidades
    class Produto
    {
        public int Id { get; }
        public string Nome { get; }
        public decimal Preco { get; }
        public string Categoria { get; }

        public Produto(int Id, string nome, decimal preco, string categoria)
        {
            if (preco <= 0)
            {
                throw new ArgumentException("Preço Invalido");
            }

            Id = Id;
            Nome = nome;
            Preco = preco;
            Categoria = categoria;

        }
    }

    class Cliente
    {
        public int Id { get; }
        public string Nome { get; }
        public string Email { get; }
        public string CPF { get; }

        public Cliente(int id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = email;
            CPF = cpf;
        }
    }

    class ItemPedido
    {
        public Produto Produto { get; }
        public int Quantidade { get; }

        public ItemPedido(Produto produto, int quantidade)
        {
            Produto = produto;
            Quantidade = quantidade;
        }

        public decimal Subtotal()
        {
            return Produto.Preco * Quantidade;
        }

    }

    // DESCONTOS

    interface IDescontoStrategy
    {
        decimal CalcularDescontos(ItemPedido item);
    }



    class SemDesconto : IDescontoStrategy
    {
        public decimal CalcularDescontos(ItemPedido item) => 0;
    }

    class DescontoCategoriaEletronica : IDescontoStrategy
    {
        public decimal CalcularDesconto(ItemPedido item)
        {
            if (item.Produto.Categoria == "Eletronico")
            {
                return item.Subtotal() + 0.10m;
            }
            return 0;
        }
    }

    class DescontoQuatidade : IDescontoStrategy
    {
        public decimal CalcularDesconto(ItemPedido item)
        {
            if (item.Quantidade >= 3)
            {
                return item.Subtotal() * 0.15m; //esse "m" com ele eu to forçando o tipo decimal
            }
            return 0;
        }
    }

    //PEDIDOS

    class Pedido
    {
        public int Id { get; }
        public Cliente Cliente { get; }
        public Lista<ItemPedido> itens { get; }
        public DateTime data { get; }
        public decimal Total { get; }

        public Pedido(int id, Cliente cliente, Lista<ItemPedido> itens, IDescontoStrategy desconto)
        {
            Id = id;
            Cliente = cliente;
            itens = itens;
            Data = DataTime.Now;
            Total = CalcularTotal(itens, desconto);
        }

        private decimal CalcularTotal(Lista<ItemPedido> itens, IDescontoStrategy desconto)
        {
            decimal total = 0;
            foreach (var item in itens)
            {
                var descontoAplicado = desconto.CalcularDescontos(item);
                total += item.Subtotal() - descontoAplicado;

            }
            return total;
        }


        //FABRICA


        static class PedidoFactory
        {
            private static int contados = 1;

            public static Pedido CriarPedido(Cliente cliente, Lista<ItemPedido> itens, IDescontoStrategy desconto)
            {
                var pedido = new Pedido(contador, cliente, itens, desconto);
                contador++;
                return pedido;
            }
        }
    }

    //REPOSITORIO

    class PedidoRepository
    {
        private Lista<Pedido> pedidos = new Lista<Pedido>();

        public void Salvar(Pedido pedido)
        {
            pedidos.Add(pedido);
        }

        public Lista<Pedido> Listar()
        {
            return pedidos;
        }
    }

    class Logger
    {
        public void Log(string mensagem)
        {
            Console.WriteLine($"[LOG] {mensagem}");
        }

    }

    class PedidoService
    {
        private PedidoRepository repo;
        private Logger logger;

        public PedidoService(PedidoRepository repo, Logger logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public Pedido CriarPedido(Cliente cliente, Lista<ItemPedido> itens, IDescontoStrategy desconto)
        {
            var pedido = PedidoFactory.CriarPedido(cliente, itens, desconto);
            repo.Salvar(pedido);
            logger.Log($"Pedido criado - ID: {pedido.Id}, Cliente: {cliente.Nome}");
            return pedido;
        }

        public void ListarPedidos()
        {
            var pedidos = repo.Listar();

            foreach (var pedido in pedidos)
            {
                Console.WriteLine("=================");
                Console.WriteLine($"Pedido ID: {pedido.Id}");
                Console.WriteLine($"Cliente: {pedido.Cliente.Nome}");
                Console.WriteLine($"Data: {pedido.Data}");
                Console.WriteLine("Itens:");
                foreach (var item in pedido.Itens)
                {
                    Console.WriteLine($"- {item.Produto.Nome} x{item.Quantidade} - R$ {item,Subtotal():F2}");
                }
                Console.WriteLine($"Total com desconto: R$ {pedido.Total:F2}");
                Console.WriteLine("=================");


            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                var repo = new PedidoRepository();
                var logger = new Logger();
                var servico = new PedidoService(repo, logger);

                var p1 = new Produto(1, "Notebook", 3000, "Eletronico");
                var p2 = new Produto(2, "Teclado", 200, "Periferico");
                var p3 = new Produto(3, "HD Externo", 500, "Eletronico");

                var cliente = new Cliente(1, "Maria", "maria@gmail.com", "12344321");

                var itens = new Lista<ItemPedido>
                {
                    new ItemPedido(p1, 1),
                    new ItemPedido(p3, 3)
                };

                IDescontoStrategy desconto = new DescontoQuatidade();

                var pedido = servico.CriarPedido(cliente, itens, desconto);

                servico.ListarPedidos();


                Console.ReadKey();
            }
        }
    }




}