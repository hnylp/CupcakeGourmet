# 🧁 Cupcake Gourmet

Sistema web de e-commerce para uma loja de cupcakes gourmet, desenvolvido como parte do **Projeto Integrador Transdisciplinar em Engenharia de Software II**, dando continuidade à modelagem UML feita no Projeto Integrador I (Situação-Problema 2 - Artefatos UML).

## Sobre o projeto

- **Back-end:** C# / ASP.NET Core 10 MVC
- **Front-end:** Razor Views + Bootstrap 5 (responsivo)
- **Banco de dados:** PostgreSQL via Entity Framework Core (Code First)
- **Hospedagem:** Render (Web Service via Docker + PostgreSQL gerenciado)
- **Autenticação:** ASP.NET Core Identity, com papéis **Cliente** e **Administrador**
- **Testes:** xUnit (testes unitários das regras de negócio de carrinho e criação de pedidos)
- **Padrão arquitetural:** MVC (Model-View-Controller), conforme orientado no material da disciplina

## Funcionalidades

**Cliente**
- Cadastro e login
- Catálogo de produtos com busca e filtro por categoria
- Carrinho de compras (sessão)
- Cadastro de endereços de entrega
- Checkout com escolha de forma de pagamento (simulado)
- Histórico de pedidos e acompanhamento visual do status da entrega

**Administrador**
- CRUD de categorias
- CRUD de produtos (com ativar/desativar)
- Listagem de pedidos com filtro por status
- Atualização do status de cada pedido (Recebido → Em preparo → Saiu para entrega → Entregue)

## Estrutura do repositório

```
CupcakeGourmet/
├── src/CupcakeGourmet.Web/     # Aplicação ASP.NET Core MVC
│   ├── Controllers/            # Controllers do site (Home, Produtos, Carrinho, Pedidos, Conta, Enderecos)
│   ├── Areas/Admin/             # Área administrativa (Categorias, Produtos, Pedidos)
│   ├── Models/                  # Entidades de domínio (Produto, Pedido, ItemPedido, Pagamento, Endereco...)
│   ├── Services/                # Regras de negócio isoladas e testáveis (CarrinhoCalculator, PedidoFactory)
│   ├── Data/                    # ApplicationDbContext, migrações do EF Core e seed de dados
│   └── Views/                   # Razor Views (Bootstrap)
├── tests/CupcakeGourmet.Tests/  # Testes unitários (xUnit)
└── docs/                        # Dicionário de dados e demais documentação
```

## Como executar localmente

Pré-requisitos: [.NET SDK 10](https://dotnet.microsoft.com/download) e um PostgreSQL local (ou aponte a connection string em `appsettings.json` para um Postgres remoto).

```bash
# Restaurar dependências e aplicar as migrações do banco
cd src/CupcakeGourmet.Web
dotnet ef database update

# Rodar a aplicação
dotnet run
```

A aplicação sobe em `http://localhost:5291` (ou a porta exibida no terminal). Na primeira execução, o sistema **popula automaticamente** o banco com categorias, produtos de exemplo e um usuário administrador, para já ficar testável.

Em produção (Render), a connection string é montada a partir das variáveis de ambiente `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER` e `DB_PASSWORD`, injetadas automaticamente pelo `render.yaml`.

### Conta de administrador (demonstração)

```
E-mail: admin@cupcakegourmet.com.br
Senha:  Admin@123
```

Qualquer outra conta criada pela tela de cadastro recebe automaticamente o papel **Cliente**.

## Rodando os testes

```bash
dotnet test
```

## Manual de uso rápido

1. **Como cliente:** acesse `/Conta/Registrar` para criar uma conta → navegue no **Catálogo** → clique em **Adicionar** para colocar produtos no carrinho → em **Carrinho**, ajuste quantidades e clique em **Finalizar pedido** → cadastre um endereço (se ainda não tiver) → escolha a forma de pagamento → **Confirmar pedido**. Acompanhe o status em **Meus Pedidos**.
2. **Como administrador:** faça login com a conta de demonstração acima → use o menu **Administração** para gerenciar Categorias, Produtos e Pedidos (inclusive atualizar o status de entrega de cada pedido).

## Documentação adicional

- [`docs/dicionario-de-dados.md`](docs/dicionario-de-dados.md) — dicionário de dados completo do projeto físico do banco.
- [`docs/uml/`](docs/uml/README.md) — revisão dos artefatos UML do PIT I (casos de uso, classes, sequência), atualizados a partir do sistema realmente implementado.

## Testes com colegas e laudo de qualidade

Os resultados dos testes realizados por colegas, o laudo de qualidade e os vídeos de demonstração são entregues separadamente conforme o formulário oficial da atividade (`PIT_atividade.docx`), e referenciados a partir deste repositório.
