# Diagrama de Classes — Cupcake Gourmet

Reflete exatamente as entidades de `src/CupcakeGourmet.Web/Models`. Corresponde ao projeto lógico normalizado descrito no [dicionário de dados](../dicionario-de-dados.md).

```mermaid
classDiagram
    class Cliente {
        <<ApplicationUser>>
        +string Id
        +string NomeCompleto
        +string Email
        +string PasswordHash
    }

    class Endereco {
        +int Id
        +string Apelido
        +string Cep
        +string Logradouro
        +string Numero
        +string Complemento
        +string Bairro
        +string Cidade
        +string Uf
    }

    class Categoria {
        +int Id
        +string Nome
        +string Descricao
    }

    class Produto {
        +int Id
        +string Nome
        +string Descricao
        +decimal Preco
        +string ImagemUrl
        +int QuantidadeEmEstoque
        +bool Ativo
    }

    class Pedido {
        +int Id
        +DateTime DataPedido
        +StatusPedido Status
        +decimal ValorTotal
    }

    class ItemPedido {
        +int Id
        +int Quantidade
        +decimal PrecoUnitario
        +decimal Subtotal
    }

    class Pagamento {
        +int Id
        +FormaPagamento Forma
        +StatusPagamento Status
        +decimal ValorPago
        +DateTime DataPagamento
    }

    class StatusPedido {
        <<enumeration>>
        Recebido
        EmPreparo
        SaiuParaEntrega
        Entregue
        Cancelado
    }

    class FormaPagamento {
        <<enumeration>>
        CartaoCredito
        Pix
        DinheiroNaEntrega
    }

    Cliente "1" --> "0..*" Endereco : possui
    Cliente "1" --> "0..*" Pedido : realiza
    Endereco "1" --> "0..*" Pedido : recebe
    Categoria "1" --> "0..*" Produto : classifica
    Pedido "1" --> "1..*" ItemPedido : contém
    Produto "1" --> "0..*" ItemPedido : é referenciado em
    Pedido "1" --> "1" Pagamento : possui
    Pedido ..> StatusPedido : usa
    Pagamento ..> FormaPagamento : usa
```

## Notas de modelagem

- `Cliente` é implementado como `ApplicationUser` (ASP.NET Core Identity) — o papel Cliente/Administrador é controlado por roles do Identity, não por uma classe separada.
- `ItemPedido.PrecoUnitario` guarda o preço no momento da compra (histórico), evitando dependência transitiva do preço atual do `Produto` — mantém a 3ª Forma Normal.
- `Pedido` e `Pagamento` têm relacionamento 1:1 — cada pedido tem exatamente um registro de pagamento.
