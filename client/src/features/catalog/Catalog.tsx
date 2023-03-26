import { useState, useEffect } from "react";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";
import ProductList from "./ProductList";


// this component holds the entire products and its components
export default function Catalog() {
     // we using useState which takes as a parameter an initial state value (in our case an empty array) and by adding the annotation <Product[]>
  // we are telling that this empty array will contain product objects.
  const [products, setProducts] = useState<Product[]>([]);
  const [loading,setLoading] = useState(true);
  // we using useEffect to fetch data from the specified url when the useState is loaded 
  useEffect(() => {
    agent.Catalog.list()
    .then(products => setProducts(products))
    .catch(error => console.log(error))
    .finally(() => setLoading(false))
  },[])

  if(loading) return <LoadingComponent message='Loading products...'/>
 
    return (
        <>
             <ProductList products = {products} />
            
        </>

    )
}