'use client';

import {useOrderContext} from "@/context/order-context";
import {withOrderItemGuard} from "@/guards/with-order-item-guard";
import {OrderItemRow} from "@/components/order/order-item-row";
import {OrderItemList} from "@/components/order/order-item-list";
import {OrderSummary} from "@/components/order/order-summary";
import {OrderTotal} from "@/components/order/order-total";
import {useGoToPayment} from "@/hooks/order/use-go-to-payment";

const OrderPage = () => {
    const {orderItems, total, changeQuantity, removeItem} = useOrderContext();
    const {goToPayment} = useGoToPayment(orderItems);

    return (
        <div className="container mx-auto px-4 py-8 flex flex-col h-[100vh] max-h-[100vh]">
            <h1 className="text-3xl font-bold mb-6 flex-shrink-0">Оформление заказа</h1>

            <div className="flex-shrink-0 pr-8 border-b border-gray-300 pb-4 mb-4">
                <OrderItemRow isHeader/>
            </div>

            <section
                aria-label="Список товаров в корзине"
                className="flex-grow overflow-y-auto space-y-4 pr-4  min-h-[100px]"
                style={{scrollbarGutter: 'stable'}}
            >
                <OrderItemList
                    items={orderItems}
                    onChangeQuantity={changeQuantity}
                    onRemove={removeItem}
                />
            </section>
            <div className="mt-auto border-t border-gray-300 pt-4">
                <OrderTotal total={total}/>

                <OrderSummary
                    total={total}
                    isDisabled={orderItems.length === 0}
                    onGoBack={() => window.history.back()}
                    onPay={goToPayment}
                />
            </div>
        </div>
    );
};

export default withOrderItemGuard(OrderPage);
