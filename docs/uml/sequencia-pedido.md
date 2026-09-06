# Diagrama de Sequência — Realizar Pedido (Checkout)

Fluxo correspondente a `PedidosController.Checkout` (`POST /Pedidos/Checkout`), usando `PedidoFactory` para montar a entidade antes de persistir.

```mermaid
sequenceDiagram
    actor Cliente
    participant View as Checkout (View)
    participant Controller as PedidosController
    participant Factory as PedidoFactory
    participant DB as ApplicationDbContext

    Cliente->>View: Confirma endereço e forma de pagamento
    View->>Controller: POST /Pedidos/Checkout
    Controller->>Controller: Valida que o endereço pertence ao cliente
    Controller->>Factory: CriarPedido(clienteId, enderecoId, itens, formaPagamento)
    Factory-->>Controller: Pedido + Itens + Pagamento (em memória)
    Controller->>DB: Add(pedido) + SaveChangesAsync()
    DB-->>Controller: Pedido persistido com Id gerado
    Controller->>Controller: Remove carrinho da sessão
    Controller-->>Cliente: Redireciona para Confirmação do pedido
```
