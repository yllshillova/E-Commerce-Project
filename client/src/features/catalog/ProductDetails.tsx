import { Divider, Grid, Typography, Table,TableContainer,TableBody, TableRow,TableCell } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import agent from "../../app/api/agent";
import NotFound from "../../app/errors/NotFound";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";

export default function ProductDetails(){
    // e kem bo id: string sepse vjen ne url e del si string perndryshe e dim qe osht number.
    const {id}= useParams<{id: string}>();
    const [product,setProduct] = useState<Product | null>(null);
    const [loading,setLoading] = useState(true);
    // ne ket rast kem perdor axios per me fetch data, si dependency e kom dergu id per arsyje se kjo ko mu thirr sa her te montohet komponenti
    //po ashtu edhe kur dependency parameter changes dmth nese niher e prekum 5 tani 3 kjo thirret edhe e kthen produktin e caktuar.
    useEffect(()=>{
        // e shtojm ket pjesen id && so this code will be executed once we actually have something in the id
        id && agent.Catalog.details(parseInt(id))
        .then(response => setProduct(response))
        .catch(error => console.log(error))
        .finally(() => setLoading(false));
    },[id])

    if(loading) return <LoadingComponent message='Loading product...' />

    if(!product) return <NotFound />

    return (
        <Grid container spacing={6}>
            <Grid item xs={6}>
                <img src={product.pictureUrl} alt={product.name} style ={{width:'100%'}} />
            </Grid>
            <Grid item xs={6}>
                <Typography variant='h3'>{product.name}</Typography>
                <Divider sx={{mb: 2}} />
                <Typography variant='h4' color='secondary'>${(product.price /100).toFixed(2)}</Typography>
                <TableContainer>
                    <Table>
                        <TableBody>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>{product.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>{product.description}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Type</TableCell>
                                <TableCell>{product.type}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Brand</TableCell>
                                <TableCell>{product.brand}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Quantity in stock</TableCell>
                                <TableCell>{product.quantityInStock}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>    
            </Grid>
        </Grid>
    )
}