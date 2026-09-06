# Dicionário de Dados — Cupcake Gourmet

Banco de dados: **SQL Server** (LocalDB em desenvolvimento), gerado via **Entity Framework Core 10 (Code First)** a partir das classes de domínio em `src/CupcakeGourmet.Web/Models`. As migrações versionadas ficam em `src/CupcakeGourmet.Web/Data/Migrations`.

## Categorias

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único da categoria |
| Nome | nvarchar(60) | Não | Nome exibido no catálogo (ex.: "Cupcakes Tradicionais") |
| Descricao | nvarchar(200) | Sim | Texto curto descrevendo a categoria |

## Produtos

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único do produto |
| Nome | nvarchar(80) | Não | Nome do cupcake |
| Descricao | nvarchar(500) | Sim | Descrição detalhada exibida na página do produto |
| Preco | decimal(10,2) | Não | Preço unitário de venda |
| ImagemUrl | nvarchar(300) | Sim | URL da imagem usada no catálogo |
| QuantidadeEmEstoque | int | Não | Estoque disponível |
| Ativo | bit | Não | Indica se o produto aparece no catálogo público |
| CategoriaId | int (FK → Categorias.Id) | Não | Categoria à qual o produto pertence |

## Enderecos

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único do endereço |
| ClienteId | nvarchar(450) (FK → AspNetUsers.Id) | Não | Cliente dono do endereço |
| Apelido | nvarchar(40) | Não | Apelido informado pelo cliente (ex.: "Casa") |
| Cep | nvarchar(9) | Não | CEP |
| Logradouro | nvarchar(120) | Não | Rua/avenida |
| Numero | nvarchar(10) | Não | Número do imóvel |
| Complemento | nvarchar(60) | Sim | Complemento (apto, bloco etc.) |
| Bairro | nvarchar(60) | Não | Bairro |
| Cidade | nvarchar(60) | Não | Cidade |
| Uf | nvarchar(2) | Não | Unidade federativa |

## Pedidos

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único do pedido |
| ClienteId | nvarchar(450) (FK → AspNetUsers.Id) | Não | Cliente que fez o pedido |
| EnderecoEntregaId | int (FK → Enderecos.Id) | Não | Endereço de entrega escolhido |
| DataPedido | datetime2 | Não | Data/hora em que o pedido foi criado |
| Status | int (enum `StatusPedido`) | Não | 0=Recebido, 1=EmPreparo, 2=SaiuParaEntrega, 3=Entregue, 4=Cancelado |
| ValorTotal | decimal(10,2) | Não | Soma dos subtotais dos itens do pedido |

## ItensPedido

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único do item |
| PedidoId | int (FK → Pedidos.Id) | Não | Pedido ao qual o item pertence |
| ProdutoId | int (FK → Produtos.Id) | Não | Produto comprado |
| Quantidade | int | Não | Quantidade solicitada |
| PrecoUnitario | decimal(10,2) | Não | Preço do produto no momento da compra (histórico, não recalcula se o preço mudar depois) |

## Pagamentos

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | int (PK, identity) | Não | Identificador único do pagamento |
| PedidoId | int (FK → Pedidos.Id, único) | Não | Pedido associado (1:1) |
| Forma | int (enum `FormaPagamento`) | Não | 0=CartaoCredito, 1=Pix, 2=DinheiroNaEntrega |
| Status | int (enum `StatusPagamento`) | Não | 0=Pendente, 1=Aprovado, 2=Recusado |
| ValorPago | decimal(10,2) | Não | Valor confirmado no pagamento |
| DataPagamento | datetime2 | Sim | Data/hora da confirmação do pagamento |

## AspNetUsers (Cliente / Administrador — ASP.NET Core Identity)

Tabela gerenciada pelo Identity; estende com o campo customizado:

| Campo | Tipo | Nulo? | Descrição |
|---|---|---|---|
| Id | nvarchar(450) (PK) | Não | Identificador único do usuário |
| Email / UserName | nvarchar(256) | Não | E-mail usado para login |
| NomeCompleto | nvarchar(max) | Não | Nome completo do usuário (campo customizado, `ApplicationUser`) |
| PasswordHash | nvarchar(max) | Não | Hash da senha (nunca armazenada em texto plano) |

O papel (**Cliente** ou **Administrador**) é controlado pelas tabelas padrão do Identity `AspNetRoles` e `AspNetUserRoles`, correspondendo aos atores "Cliente" e "Administrador" do diagrama de casos de uso do PI I.

## Relacionamentos (resumo)

- Categoria **1 — N** Produto
- Cliente (AspNetUsers) **1 — N** Endereco
- Cliente (AspNetUsers) **1 — N** Pedido
- Endereco **1 — N** Pedido (um endereço pode ser reutilizado em vários pedidos)
- Pedido **1 — N** ItemPedido
- Produto **1 — N** ItemPedido
- Pedido **1 — 1** Pagamento

## Normalização

O modelo está na **3ª Forma Normal (3FN)**:
- Não há grupos repetitivos (itens de pedido ficam em tabela própria `ItensPedido`, não em colunas repetidas dentro de `Pedidos`).
- Todo atributo não-chave depende exclusivamente da chave primária de sua tabela (ex.: `PrecoUnitario` fica em `ItensPedido`, não em `Pedidos`, pois é uma propriedade da combinação pedido+produto, não do pedido como um todo).
- Não há dependências transitivas: por exemplo, o nome/preço do produto não é duplicado em `ItensPedido` (apenas a referência `ProdutoId` e o preço histórico no momento da compra, que é uma informação legítima do item, não uma cópia redundante do cadastro).
