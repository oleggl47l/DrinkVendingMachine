'use client';

import {useRouter} from "next/navigation";
import {usePaymentSuccess} from "@/hooks/use-payment-success";
import {PaymentSuccessView} from "@/components/payment-success/payment-success-view";

export default function PaymentSuccessPage() {
    const router = useRouter();
    const {changeAmount, changeCoins, clearStorage} = usePaymentSuccess();

    if (changeAmount === null) return null;

    const handleReturn = () => {
        clearStorage();
        router.push('/catalog');
    };

    return (
        <PaymentSuccessView
            changeAmount={changeAmount}
            changeCoins={changeCoins}
            onReturnToCatalog={handleReturn}
        />
    );
}
