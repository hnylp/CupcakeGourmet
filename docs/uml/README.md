# Documentação UML - Revisão PIT II

Esta pasta contém a **revisão e atualização** dos artefatos UML produzidos no Projeto Integrador Transdisciplinar em Engenharia de Software I (Situação-Problema 2 - Artefatos UML), conforme pedido na Situação-Problema 1 do PIT II:

> "Verifique e adeque os elementos de modelagens da solução feitos em UML (diagramas de classe, casos de uso, sequência, atividades etc.), veja onde podemos colocar melhorias e alterações necessárias visando acurácia da modelagem."

## Por que os diagramas foram refeitos (e não só revisados)

Os diagramas originais do PIT I foram feitos no Lucidchart e não estão mais acessíveis. Em vez de recriar de memória o que foi planejado, os diagramas abaixo foram construídos a partir do **sistema realmente implementado**. Portanto, refletem com precisão as entidades, atributos, relacionamentos e fluxos que existem no código-fonte (`src/CupcakeGourmet.Web`), o que atende com mais rigor ao pedido de "acurácia da modelagem".

Os diagramas usam a sintaxe [Mermaid](https://mermaid.js.org/), renderizada automaticamente pelo GitHub ao visualizar estes arquivos `.md`.

## Diagramas

- [Diagrama de Casos de Uso](casos-de-uso.md): atores (Cliente, Administrador) e funcionalidades do sistema
- [Diagrama de Classes](diagrama-classes.md): entidades de domínio, atributos e relacionamentos
- [Diagrama de Sequência — Login](sequencia-login.md)
- [Diagrama de Sequência — Realizar Pedido](sequencia-pedido.md)

Para o projeto físico do banco de dados e o dicionário de dados, veja [`../dicionario-de-dados.md`](../dicionario-de-dados.md).

## Principais diferenças em relação ao planejado no PIT I

- O ator **"Sistema de Pagamento"** citado no PIT I foi simplificado: o pagamento é registrado e aprovado automaticamente pelo próprio sistema (simulação), sem integração com um gateway externo real — refletido no diagrama de classes como uma entidade `Pagamento`, não como um ator externo.
- A entidade `Cliente` é implementada como `ApplicationUser` (ASP.NET Core Identity), estendida com o campo `NomeCompleto`, e o papel de Cliente/Administrador é controlado por perfis (roles) do Identity.
- Casos de uso administrativos (gerenciar categorias, produtos e status de pedidos) foram incluídos, pois não estavam detalhados nos casos de uso expandidos do PIT I.
