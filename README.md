# DiscountManagement
The project has three tables: stores, products and storeproducts(bridging table).
The stores and products tables have a many-to-many relationship. That means, a store can sell many products. A product can be sold in many stores.
You can create, read, update and delete stores or products. 

A store has a store-id, store_name, discount_code, discount_rate and a picture.
A product has a product_id, product_name, price and URL.

DONE: 
API call for listing all products associated with a store
API call for listing all stores that a product can be bought at
API call for adding an association between a store and a product
API call for removing an association between a product and a store
Views which support the above API calls as interfaces.
Store Image upload feature

