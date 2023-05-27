import { LoadingButton } from "@mui/lab";
import { Divider, Grid, Typography, Table, TableContainer, TableBody, TableRow, TableCell, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import agent from "../../app/api/agent";
import { useStoreContext } from "../../app/context/StoreContext";
import NotFound from "../../app/errors/NotFound";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";

export default function ProductDetails() {
    const { basket, setBasket, removeItem } = useStoreContext();
    // e kem bo id: string sepse vjen ne url e del si string perndryshe e dim qe osht number.
    const { id } = useParams<{ id: string }>();
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [quantity, setQuantity] = useState(0);
    const [submitting, setSubmitting] = useState(false);
    const item = basket?.items.find(i => i.productId === product?.id);

    // ne ket rast kem perdor axios per me fetch data, si dependency e kom dergu id per arsyje se kjo ko mu thirr sa her te montohet komponenti
    //po ashtu edhe kur dependency parameter changes dmth nese niher e prekum 5 tani 3 kjo thirret edhe e kthen produktin e caktuar.
    useEffect(() => {
        if (item) setQuantity(item.quantity);
        // e shtojm ket pjesen id && so this code will be executed once we actually have something in the id
        id && agent.Catalog.details(parseInt(id))
            .then(response => setProduct(response))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
    }, [id, item])
    // funksioni qe e mundeson ndryshimin e numrit te quantity
    function handleInputChange(event: any) {
        if (event.target.value >= 0) {
            setQuantity(parseInt(event.target.value));
        }
    }
    // metoda e cila e permban logjiken e shtimit dhe hekjes se produkteve nga shporta ku ifi i par permban logjiken nese nuk ekziston produkti ne shporte
    // apo nese quantity i dhene eshte me i madh se ati i item qe e ka vet (nese e kem shtu kuantitetin vet) ather shtohet ne basket duket u rujt vlera
    // ne updatedQuantity. e nese nuk eshte asnjona kalon ne else ku e fshin sasin .
    function handleUpdateCart(){
        setSubmitting(true);
        if(!item || quantity > item.quantity){
            const updatedQuantity = item ? quantity - item.quantity : quantity;
            agent.Basket.addItem(product?.id!, updatedQuantity)
                .then(basket => setBasket(basket))
                .catch(error => console.log(error))
                .finally(() => setSubmitting(false))
        } 
        else{
            const updatedQuantityy = item.quantity - quantity;
            agent.Basket.removeItem(product?.id!, updatedQuantityy)
                .then(() => removeItem(product?.id!, updatedQuantityy))
                .catch(error => console.log(error))
                .finally(() => setSubmitting(false))
        }
    }

    if (loading) return <LoadingComponent message='Loading product...' />

    if (!product) return <NotFound />

    return (
        <Grid container spacing={6}>
            <Grid item xs={6}>
                <img src={product.pictureUrl} alt={product.name} style={{ width: '100%' }} />
            </Grid>
            <Grid item xs={6}>
                <Typography variant='h3'>{product.name}</Typography>
                <Divider sx={{ mb: 2 }} />
                <Typography variant='h4' color='secondary'>${(product.price / 100).toFixed(2)}</Typography>
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
                <Grid container spacing={2}>
                    <Grid item xs={6}>
                        <TextField
                            onChange={handleInputChange}
                            variant='outlined'
                            type='number'
                            label='Quantity in Cart'
                            fullWidth
                            value={quantity}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <LoadingButton
                            disabled={item?.quantity === quantity || !item && quantity === 0}
                            loading={submitting}
                            onClick={handleUpdateCart}
                            sx={{ height: '55px' }}
                            color='primary'
                            size='large'
                            variant='contained'
                            fullWidth
                        >
                            {item ? 'Update Quantity' : 'Add to Cart'}
                        </LoadingButton>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}