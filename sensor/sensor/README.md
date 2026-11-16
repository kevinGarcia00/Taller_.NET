# Endpoints del sensor

## Recuperar un inversor
```bash
curl http://localhost:5023/api/inversor/{idInversor}
```

## Consultar un panel
```bash
curl http://localhost:5023/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}
```

## Encender un panel
```bash
curl -X PUT http://localhost:5023/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado \
  -H "Content-Type: application/json" \
  -d '{"accion":"Encender"}'
```

## Apagar un panel
```bash
curl -X PUT http://localhost:5023/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado \
  -H "Content-Type: application/json" \
  -d '{"accion":"Apagar"}'
```

## Habilitar volcado de energia de un panel
```bash
curl -X PUT http://localhost:5023/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado \
  -H "Content-Type: application/json" \
  -d '{"volcarEnergia":true}'
```

## Deshabilitar volcado de energia de un panel
```bash
curl -X PUT http://localhost:5023/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado \
  -H "Content-Type: application/json" \
  -d '{"volcarEnergia":false}'
```
