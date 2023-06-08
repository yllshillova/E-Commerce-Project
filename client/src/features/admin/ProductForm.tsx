import { Typography, Grid, Paper, Box, Button } from "@mui/material";
import { useEffect } from "react";
import { FieldValues, useForm } from "react-hook-form";
import AppDropzone from "../../app/components/AppDropZone";
import AppSelectList from "../../app/components/AppSelectList";
import useProducts from "../../app/hooks/useProducts";
import { Product } from "../../app/models/product";
import {yupResolver} from '@hookform/resolvers/yup';
import { validationSchema } from "./productValidation";
import AppTextInput from "../../app/components/AppTextInput";
import agent from "../../app/api/agent";
import { useAppDispatch } from "../../app/store/configureStore";
import { setProduct } from "../catalog/catalogSlice";
import { LoadingButton } from "@mui/lab";
interface Props {
    product?: Product;
    cancelEdit: () => void;
}


export default function ProductForm({ product, cancelEdit }: Props) {
    const { control, reset, handleSubmit, watch, formState:{isDirty, isSubmitting} } = useForm({
        resolver: yupResolver<any>(validationSchema)
    });
    const { brands, types } = useProducts();
    const watchFile = watch('file', null);
    const dispatch = useAppDispatch();
    useEffect(() => {
        if (product && !watchFile && !isDirty) reset(product);
        return () =>{
            if(watchFile) URL.revokeObjectURL(watchFile.preview);
        }
    }, [product, reset, watchFile, isDirty]);

    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Product;
            if (product) {
                response = await agent.Admin.updateProduct(data);
            } else {
                response = await agent.Admin.createProduct(data);
            }
            dispatch(setProduct(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }
    return (
        <Box component={Paper} sx={{ p: 4 }}>
        <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
            Product Details
        </Typography>
        <form onSubmit={handleSubmit(handleSubmitData)}>
            <Grid container spacing={3}>
                <Grid item xs={12} sm={12}>
                    <AppTextInput control={control} name='name' label='Product name' />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppSelectList control={control} items={brands} name='brand' label='Brand' />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppSelectList control={control} items={types} name='type' label='Type' />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppTextInput type='number' control={control} name='price' label='Price'  />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppTextInput  type='number' control={control} name='quantityInStock' label='Quantity in Stock' />
                </Grid>
                <Grid item xs={12}>
                    <AppTextInput control={control} multiline={true} rows={4} name='description' label='Description' />
                </Grid>
                    <Grid item xs={12}>
                        {/* normalisht kjo emri i image o pictureUrl po n createproductdto osht IFormFile file */}
                        <Box display='flex' justifyContent='space-between' alignItems='center'>
                            <AppDropzone control={control} name='file' />
                            {watchFile ? (
                                <>
                                    <Typography variant='h5'>Product Image preview:</Typography>
                                    {watchFile.preview ? (
                                        <img
                                            src={watchFile.preview}
                                            alt="preview"
                                            style={{
                                                maxHeight: 200,
                                                border: 'dashed 3px #eee',
                                                borderColor: '#eee',
                                                borderRadius: '5px'
                                            }}
                                        />
                                    ) : (
                                        <div
                                            style={{
                                                maxHeight: 200,
                                                border: 'dashed 3px #eee',
                                                borderColor: '#eee',
                                                borderRadius: '5px',
                                                display: 'flex',
                                                justifyContent: 'center',
                                                alignItems: 'center'
                                            }}
                                        >
                                            <span
                                                style={{
                                                    color: '#bbb',
                                                    fontWeight: 'bold'
                                                }}
                                            >
                                                No image preview yet!
                                            </span>
                                        </div>
                                    )}
                                </>
                            ) : (
                                <>
                                    <Typography variant='h5'>Product Image preview:</Typography>
                                    {product?.pictureUrl ? (
                                        <img
                                            src={product.pictureUrl}
                                            alt={product.name}
                                            style={{
                                                maxHeight: 200,
                                                border: 'dashed 3px #eee',
                                                borderColor: '#eee',
                                                borderRadius: '5px'
                                            }}
                                        />
                                    ) : (
                                        <div
                                            style={{
                                                maxHeight: 200,
                                                border: 'dashed 3px #eee',
                                                borderColor: '#eee',
                                                borderRadius: '5px',
                                                display: 'flex',
                                                justifyContent: 'center',
                                                alignItems: 'center'
                                            }}
                                        >
                                            <span
                                                style={{
                                                    color: '#bbb',
                                                    fontWeight: 'bold'
                                                }}
                                            >
                                                No image preview yet!
                                            </span>
                                        </div>
                                    )}
                                </>
                            )}
                        </Box>
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ mt: 3 }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit'>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}