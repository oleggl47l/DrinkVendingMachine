'use client';

import {useRouter} from "next/navigation";
import {usePaymentSuccess} from "@/hooks/use-payment-success";
import {PaymentSuccessView} from "@/components/payment-success/payment-success-view";
import {Loading} from "@/components/ui/loading";

export default function PaymentSuccessPage() {
    const router = useRouter();
    const {changeAmount, changeCoins, clearStorage} = usePaymentSuccess();

    if (changeAmount === null) return <Loading />;

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
