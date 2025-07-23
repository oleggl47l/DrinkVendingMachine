import {useOrderContext} from "@/context/order-context";
import {useRouter} from "next/navigation";
import {DrinkModel} from "@/app/api/drink-vending-machine";

export const useDrinkSelection = (drinks: DrinkModel[], selectedDrinkIds: Set<number>) => {
    const router = useRouter();
    const {addItem} = useOrderContext();

    const goToOrderPage = () => {
        const selected = drinks.filter(d => selectedDrinkIds.has(d.id!));
        const orderItems = selected.map(drink => ({
            ...drink,
            quantitySelected: 1,
        }));

        addItem(orderItems);
        router.push('/order');
    };

    return {goToOrderPage, selectedCount: selectedDrinkIds.size};
};