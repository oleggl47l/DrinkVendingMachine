'use client';

import {OrderItemRow} from '@/components/order/order-item-row';
import {useOrderContext} from "@/context/order-context";
import {useRouter} from "next/navigation";
import {useToast} from "@/components/ui/toast";

export default function OrderPage() {
    const router = useRouter();
    const {orderItems, total, changeQuantity, removeItem} = useOrderContext();
    const { showToast } = useToast();

    const handleGoToPayment = () => {
        if (orderItems.length === 0) {
            showToast('Корзина пуста. Добавьте товары, чтобы перейти к оплате.', 'error');
            return;
        }
        router.push('/payment');
    };

    return (
        <div className="container mx-auto px-4 py-8 flex flex-col h-[100vh] max-h-[100vh]">
            <h1 className="text-3xl font-bold mb-6 flex-shrink-0">Оформление заказа</h1>

            <div className="flex-shrink-0 pr-8 border-b border-gray-300 pb-4 mb-4">
                <OrderItemRow isHeader />
            </div>

            <section
                aria-label="Список товаров в корзине"
                className="flex-grow overflow-y-auto space-y-4 pr-4  min-h-[100px]"
                style={{ scrollbarGutter: 'stable' }}
            >
                {orderItems.length === 0 ? (
                    <p className="text-center text-gray-600 mt-4">Корзина пуста</p>
                ) : (
                    orderItems.map((item) => (
                        <OrderItemRow
                            key={item.id}
                            item={item}
                            onChangeQuantity={changeQuantity}
                            onRemove={removeItem}
                        />
                    ))
                )}
            </section>

            <div className="mt-auto border-t border-gray-300 pt-4">
                <div className="flex justify-end mt-2 mb-8">
                    <p>Итоговая сумма</p>
                    <p className="ml-6 text-xl font-semibold">{total} руб.</p>
                </div>

                <div className="flex justify-between">
                    <button
                        onClick={() => window.history.back()}
                        className="bg-yellow-400 text-black hover:bg-yellow-500 px-20 py-3 rounded"
                    >
                        Вернуться
                    </button>
                    <button
                        onClick={handleGoToPayment}
                        disabled={orderItems.length === 0}
                        className={`px-20 py-3 rounded ${
                            orderItems.length === 0
                                ? 'bg-gray-400 text-white disabled'
                                : 'bg-green-600 text-white hover:bg-green-700'
                        }`}
                    >
                        Оплата
                    </button>
                </div>
            </div>
        </div>
    );
}
