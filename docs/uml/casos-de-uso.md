# Diagrama de Casos de Uso — Cupcake Gourmet

Dois atores interagem com o sistema: **Cliente** (usuário final que compra) e **Administrador** (gerencia o catálogo e os pedidos). Casos de uso pontilhados indicam relação `<<include>>` — o checkout depende de haver itens no carrinho e um endereço cadastrado.

```mermaid
flowchart LR
    Cliente((👤 Cliente))
    Administrador((👤 Administrador))

    subgraph Sistema["Sistema Cupcake Gourmet"]
        UC1([Cadastrar-se / Login])
        UC2([Buscar e filtrar produtos])
        UC3([Ver detalhes do produto])
        UC4([Gerenciar carrinho])
        UC5([Cadastrar endereço de entrega])
        UC6([Finalizar pedido / Checkout])
        UC7([Acompanhar status do pedido])
        UC8([Gerenciar categorias])
        UC9([Gerenciar produtos])
        UC10([Gerenciar pedidos e status])
    end

    Cliente --> UC1
    Cliente --> UC2
    Cliente --> UC3
    Cliente --> UC4
    Cliente --> UC5
    Cliente --> UC6
    Cliente --> UC7

    Administrador --> UC1
    Administrador --> UC8
    Administrador --> UC9
    Administrador --> UC10

    UC6 -.include.-> UC4
    UC6 -.include.-> UC5
```

## Descrição dos casos de uso principais

| Caso de uso | Ator | Descrição resumida |
|---|---|---|
| Cadastrar-se / Login | Cliente, Administrador | Criação de conta e autenticação via ASP.NET Core Identity |
| Buscar e filtrar produtos | Cliente | Pesquisa por nome e filtro por categoria no catálogo |
| Gerenciar carrinho | Cliente | Adicionar, atualizar quantidade e remover itens (mantido em sessão) |
| Cadastrar endereço de entrega | Cliente | Cadastro de um ou mais endereços vinculados ao cliente |
| Finalizar pedido / Checkout | Cliente | Escolhe endereço e forma de pagamento; gera Pedido, Itens e Pagamento |
| Acompanhar status do pedido | Cliente | Visualiza o histórico e o progresso (Recebido → Em preparo → Saiu para entrega → Entregue) |
| Gerenciar categorias / produtos | Administrador | CRUD de categorias e produtos do catálogo |
| Gerenciar pedidos e status | Administrador | Lista todos os pedidos e atualiza o status de entrega |
