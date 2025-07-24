'use client';

import {usePaymentSuccess} from "@/hooks/payment-success/use-payment-success";
import {PaymentSuccessView} from "@/components/payment-success/payment-success-view";
import {Loading} from "@/components/ui/loading";
import {useReturnToCatalogAfterPayment} from "@/hooks/payment-success/use-return-to-cat-after-payment";

export default function PaymentSuccessPage() {
    const {changeAmount, changeCoins} = usePaymentSuccess();
    const {returnToCatalog} = useReturnToCatalogAfterPayment();

    if (changeAmount === null) return <Loading />;

    return (
        <PaymentSuccessView
            changeAmount={changeAmount}
            changeCoins={changeCoins}
            onReturnToCatalog={returnToCatalog}
        />
    );
}
