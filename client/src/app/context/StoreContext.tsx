import { createContext, PropsWithChildren, useContext, useState } from "react";
import { Basket } from "../models/basket";
// ky interface definon formen e vleres se kij konteksti
interface StoreContextValue {
    removeItem: (productId: number, quantity: number) => void;
    setBasket: (basket: Basket) => void;
    basket: Basket | null;
}

export const StoreContext = createContext<StoreContextValue | undefined>(undefined);
// ktu e kem kriju nje hook 
export function useStoreContext() {
    let context = useContext(StoreContext);

    if (context === undefined) {
        throw Error('Oops - we do not seem to be inside the provider');
    }

    return context;
}

export function StoreProvider({ children }: PropsWithChildren<any>) {
    const [basket, setBasket] = useState<Basket | null>(null);

    function removeItem(productId: number, quantity: number) {
        if (!basket) return;
        // kjo '...'-spread operator krijon nje kopje te array dhe e store ne items.
        const items = [...basket.items];
        // ktu e marrum ni produkt tcaktum me index qe gjindet ne shport
        const itemIndex = items.findIndex(i => i.productId === productId);
        // nese ka produkte nshport ather hin nkusht edhe bohet reduce quantity pra zvoglohet
        // nese quantity osht 0 ather fshihet produkti prej array ne rastin ton item qe gjindet ne itemIndex
        if (itemIndex >= 0) {
            items[itemIndex].quantity -= quantity;
            //                                     e fshin nje element nga array
            if (items[itemIndex].quantity === 0) items.splice(itemIndex, 1);
            setBasket(prevState => {
                return {...prevState!, items}
            })
        }
    }
    return (
        <StoreContext.Provider value={{basket, setBasket, removeItem}}>
            {children}
        </StoreContext.Provider>
    )
}




// This code allows components to access the basket state, update it using the setBasket function
//  and remove items from the basket using the removeItem function, all through the StoreContext
// and the useStoreContext hook.
