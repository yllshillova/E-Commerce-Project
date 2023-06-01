import { useEffect } from "react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { fetchProductsAsync, productSelectors } from "./catalogSlice";
import ProductList from "./ProductList";


// this component holds the entire products and its components
export default function Catalog() {
  // kjo 'products' naj kthen ni list me produkte
  const products = useAppSelector(productSelectors.selectAll);
  const {productsLoaded, status} = useAppSelector(state => state.catalog);
  const dispatch = useAppDispatch();
  // we using useEffect to fetch data from the specified url when the useState is loaded 
  useEffect(() => {
    if(!productsLoaded) dispatch(fetchProductsAsync());
  },[productsLoaded, dispatch])

  if(status.includes('pending')) return <LoadingComponent message='Loading products...'/>
 
    return (
        <>
             <ProductList products = {products} />
            
        </>

    )
}