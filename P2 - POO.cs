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

}