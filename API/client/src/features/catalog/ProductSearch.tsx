import { debounce, TextField } from "@mui/material";
import { useState } from "react";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { setProductParams } from "./catalogSlice";

export default function ProductSearch(){
    const {productParams} = useAppSelector(state => state.catalog);
    const [searchTerm, setSearchTerm] = useState(productParams.searchTerm);
    const dispatch = useAppDispatch();

    const debouncedSearch = debounce((event: any) => {
        dispatch(setProductParams({searchTerm: event.target.value}))
    }, 1000);

    return (
        <TextField
            label='Search products'
            variant='outlined'
            fullWidth
            value={searchTerm || ''}
            onChange={(event : any) => {
                setSearchTerm(event.target.value);
                debouncedSearch(event);
            }}
          />
    )
}
// qka ndodh ne onchange?
// when we dispatch this action we re gonna set our product params inside our catalogSlice and when we set our product params then we re gonna 
// change our state of the productLoaded to false and then we re gonna pass our action.payload in that case the search term and override that part of state inside our product params
// than our productloaded variable value is gonna change and thats a dependency of our useEffect so when this changes it forces our useEffect to run again
// so the condition is gonna force do dispatch(fetchProductsAsync()) and when we dispatch that action  then we re gonna get our Axiosparams
// in our case it gonna contain the searchTerm and the other ones such as page number, pagesize etc and its gonna make the request for these params.