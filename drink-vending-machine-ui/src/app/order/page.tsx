'use client';

import {OrderItemRow} from '@/components/order/order-item-row';
import {useOrder} from '@/hooks/use-order';

export default function OrderPage() {
    const {orderItems, total, changeQuantity, removeItem} = useOrder();

    return (

        <main className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-6">Оформление заказа</h1>

            <section
                aria-label="Заголовок корзины с товарами"
                className="grid grid-cols-[1fr_300px_150px_40px] gap-4 font-semibold mb-4"
            >
                <div>Товар</div>
                <div>Количество</div>
                <div>Цена</div>
                <div aria-hidden="true"></div>
            </section>

            <section aria-label="Список товаров в корзине" className="border-t border-gray-300 mt-4 pt-4">
                {orderItems.length === 0 ? (
                    <p>Корзина пуста</p>
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
            <div className="border-t border-gray-300 mt-4 pt-4">
                <div className="flex justify-end mt-4 mb-8">
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
                        onClick={() => alert('Оплата не реализована')}
                        disabled={orderItems.length === 0}
                        className={`px-20 py-3 rounded
                            ${orderItems.length === 0
                            ? 'bg-gray-400 text-white disabled'
                            : 'bg-green-600 text-white hover:bg-green-700'
                        }`}
                    >
                        Оплата
                    </button>

                </div>
            </div>

        </main>
    );
}
