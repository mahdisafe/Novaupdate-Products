# NovaShop product updates (BU50-API)

WinForms helper that syncs Focus stock/prices into NovaShop via admin product APIs.

## Paths

- Project: `BU50-API/BU50-API.csproj`
- Release EXE: `BU50-API/bin/Release/BU50-API.exe`

## Branch

- `helper/product-updates` — API base URL + sync fixes

## API

- Base: `https://api.efifty.com` (not `www.efifty.com`)
- `GET /api/admin/products/sync-list`
- `POST /api/admin/products/bulk-sync`
- Auth: `X-Api-Key`

## Related

- Focus order/invoice saver: [NOVAFocusAPI](https://github.com/mahdisafe/NOVAFocusAPI) branch `helper/invoice-orders`
