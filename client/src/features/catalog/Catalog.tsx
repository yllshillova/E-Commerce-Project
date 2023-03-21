import { useState, useEffect } from "react";
import { Product } from "../../app/models/product";
import ProductList from "./ProductList";


// this component holds the entire products and its components
export default function Catalog() {
     // we using useState which takes as a parameter an initial state value (in our case an empty array) and by adding the annotation <Product[]>
  // we are telling that this empty array will contain product objects.
  const [products, setProducts] = useState<Product[]>([]);
  // we using useEffect to fetch data from the specified url when the useState is loaded 
  useEffect(() => {
    fetch('http://localhost:5000/api/products')
    .then(response => response.json())
    .then(data => setProducts(data))
  },[])

 
    return (
        <>
             <ProductList products = {products} />
            
        </>

    )
}