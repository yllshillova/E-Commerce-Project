import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { router } from "../router/Routes";

const sleep = () => new Promise(resolve => setTimeout(resolve, 500));

axios.defaults.baseURL = 'http://localhost:5000/api/';
// kta e bojm qe me mujt browseri me pranu cookie dhe me bo set cookie ne storage tone te applikacionit.
axios.defaults.withCredentials = true;
// responseBody is equivalent to :
/* 
    function responseBodyFn(response : AxiosResponse) {
        return response.data;
    }
*/
// kthen vetem te dhenat nga pergjigja e axios
const responseBody = (response: AxiosResponse) => response.data;
// kemi perdor metoden use() ecila pranon dy parametra njerin nese kerkesa eshte e suksesshme dhe te dytin nese ndodh ndonje gabim
// gabimev ju kem qas tu perdor AxiosError response ku i qasemi JSON file duke perdor parametrat data dhe status 
// data permban fusha te caktuara ne json response qe e kemi krijuar ne exceptionmiddleware  kurse statusi varet nga errori psh 400 401 500 etc.
axios.interceptors.response.use(async response => {
    await sleep();
    return response
},(error : AxiosError) => {
    const {data, status} = error.response as AxiosResponse;
    switch(status){
        case 400:
            if(data.errors) {
                const modelStateErrors : string[] = [];
                for(const key in data.errors){
                    if(data.errors[key]){
                        modelStateErrors.push(data.errors[key])
                    }
                }
                // flat() kthen nje array me te gjitha sub-array te konkatinuar me lart.
                throw modelStateErrors.flat();
            }
            toast.error(data.title);
            break;
        case 401:
            toast.error(data.title);
            break;
        case 500:
            // navigate per me na navigu dikun 2 parametra 'to:' dhe options
            router.navigate('/server-error',{state: {error : data}});
            break;
        default: 
            break;
    }
    return Promise.reject(error.response);
})

// objekti requests krijohet qe te kryejm kerkesa ne backend duke perdor librarine axios, saher qe perdoret get put post ose delete kthehet ose data nga server ose nje gabim gjat perpunimit.
const requests = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body : {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body : {}) => axios.put(url,body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody)
}

// this is the object that will store our requests for our catalog
const Catalog = {
    list : () => requests.get('products'),
    details: (id: number) => requests.get(`products/${id}`)
}

const TestErrors = {
    get400Error : () => requests.get('buggy/bad-request'),
    get401Error : () => requests.get('buggy/unauthorised'),
    get404Error : () => requests.get('buggy/not-found'),
    get500Error : () => requests.get('buggy/server-error'),
    getValidationError : () => requests.get('buggy/validation-error'),
}

const Basket = {
    get: () => requests.get('basket'),
    // productId duhet me kan e shkrume saktsisht qishtu per arsyje se API duhet me pas ni key
    // qe i pershtatet kur t shkon te basketController ne httpPost request me kqyr.
    addItem: (productId: number, quantity = 1 ) => requests.post(`basket?productId=${productId}&quantity=${quantity}`,{}),
    removeItem: (productId: number, quantity = 1) => requests.delete(`basket?productId=${productId}&quantity=${quantity}`)

}


const agent= {
    Catalog,
    TestErrors,
    Basket
}


export default agent;



/*
    krijon nje "agent" ose nje objekt qe permban funksionet per te derguar kerkesa ne backend perdorur axios,
    dhe perdor keto funksione per te kthyer informacione nga backend-i, si dhe per te derguar informacione ne backend.
    Nga ana tjeter, permban nje objekt "Catalog" qe permban dy funksione, 
    nje per te listuar produkte dhe nje per te marre detajet e produkteve, duke perdorur funksionet e "requests" per te kryer kerkesa ne backend.
*/ 