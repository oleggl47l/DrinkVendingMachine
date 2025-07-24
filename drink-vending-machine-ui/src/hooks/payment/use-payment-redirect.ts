import {useEffect} from "react";
import {useRouter} from "next/navigation";
import {useOrderContext} from "@/context/order-context";

export function usePaymentRedirect(isPaid: boolean) {
    const router = useRouter();
    const {orderItems, isHydrated} = useOrderContext();

    useEffect(() => {
        if (!isHydrated) return;
        if (orderItems.length === 0 && !isPaid) {
            router.replace('/catalog');
        }
    }, [isHydrated, orderItems, isPaid, router]);
}
