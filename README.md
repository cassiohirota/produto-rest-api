# produto-rest-api

## Descrição
Api de gestão de categorias e produtos.

## Tecnologias
* C#
* MongoDB

## Diagrama
![](https://i.imgur.com/H7j6uLG.jpeg)

## Instruções
1 - É necessário instalar o driver mongoDB. Caso não possua, pelo PowerShell execute este comando:
```
Install-Package MongoDB.Driver
```
2 - Executar o projeto e utilizar pelo Swaggger ou por alguma ferramenta de teste de api(Postman, Imnsonia, etc)

## EndPoint
```
http://localhost:5171/
```
### Categoria
Método GET
* Lista todas as categorias registradas
```
http://localhost:5171/v1/categorias
```
* Busca uma categoria
```
http://localhost:5171/v1/categorias/{categoriaId}
```
Método POST
* Cria uma categoria
```
http://localhost:5171/v1/categorias
```
Método PUT
* Altera os dados de uma categoria
```
http://localhost:5171/v1/categorias/{categoriaId}
```
Método DELETE
* Exclui uma categoria
```
http://localhost:5171/v1/categorias/{categoriaId}
```
### Produto
Método GET
* Lista todos os produtos
```
http://localhost:5171/v1/produtos
```
* Lista todos os produtos de uma categoria
```
http://localhost:5171/v1/produtos/Categorias/{categoriaId}
```
* Lista todos os produtos por filtragem
```
http://localhost:5171/v1/produtos/filter
```
* Lista todos os produtos por paginação
```
http://localhost:5171/v1/produtos/query
```
Método POST
* Cria um produto
```
http://localhost:5171/v1/produtos
```
Método PUT
* Atualiza um produto
```
http://localhost:5171/v1/produtos/{produtoId}
```
* Atualiza a categoria de um produto
```
http://localhost:5171/v1/produtos/{produtoId}/categorias/{categoriaId}
```
Método DELETE
* Deleta um produto
```
http://localhost:5171/v1/produtos/{produtoId}
```

