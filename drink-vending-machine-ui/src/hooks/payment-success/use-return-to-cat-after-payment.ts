import {useRouter} from "next/navigation";
import {usePaymentSuccess} from "@/hooks/payment-success/use-payment-success";

export const useReturnToCatalogAfterPayment = () => {
    const router = useRouter();
    const {clearStorage} = usePaymentSuccess();

    const returnToCatalog = () => {
        clearStorage();
        router.push('/catalog');
    };

    return {returnToCatalog};
};
