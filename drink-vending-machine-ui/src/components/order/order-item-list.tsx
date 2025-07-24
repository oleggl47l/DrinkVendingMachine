import {OrderItem} from "@/context/order-context";
import {OrderItemRow} from "@/components/order/order-item-row";

interface Props {
    items: OrderItem[];
    onChangeQuantity: (id: number, delta: number) => void;
    onRemove: (id: number) => void;
}

export const OrderItemList = ({items, onChangeQuantity, onRemove}: Props) => {
    if (items.length === 0) {
        return <p className="text-center text-gray-600 mt-4">Корзина пуста</p>;
    }

    return (
        <>
            {items.map((item) => (
                <OrderItemRow
                    key={item.id}
                    item={item}
                    onChangeQuantity={onChangeQuantity}
                    onRemove={onRemove}
                />
            ))}
        </>
    );
};
