import { Grid } from "@mui/material";
import { Product } from "../../app/models/product";
import { useAppSelector } from "../../app/store/configureStore";
import ProductCard from "./ProductCard";
import ProductCardSkeleton from "./ProductCardSkeleton";
// per mos me i hup benefitet e typescript dmth mos me i bo props : typeof(any) na e bojm ni interface e aty i vendosum 
// props qe na duhet me i kthy psh ne rastin tone produktet edhe funksionin add product, tipi ju vendoset varsisht se qka kthejn
// duhet me kqyr ne parent component(App.tsx).
interface Props {
    products: Product[];
}
// this is the component that has the list of the products
export default function ProductList({ products }: Props) {
    const {productsLoaded} = useAppSelector(state => state.catalog);
    return (
        <Grid container spacing={4}>
            {products.map((product) => (
                // we put the key to the grid because this is the first element that we're looping in and that will be checked.
                <Grid item xs={4} key={product.id}>
                    {!productsLoaded ? (
                        <ProductCardSkeleton />
                        ) : (
                            
                    <ProductCard product={product} />
                    )}
                </Grid>
            ))}
        </Grid>
    )

}