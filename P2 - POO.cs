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
                return item.Subtotal() * 0.15m;
            }
            return 0;
        }
    }
    




}