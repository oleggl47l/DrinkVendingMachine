import {DrinkModel} from "@/app/api/drink-vending-machine";
import {Loading} from "@/components/ui/loading";
import {Error} from "@/components/ui/error";
import {DrinkCard} from "@/components/drinks/drink-card";

export const DrinkList = ({
                              drinks,
                              loading,
                              selectedDrinkIds,
                              onSelect
                          }: {
    drinks: DrinkModel[];
    loading: boolean;
    selectedDrinkIds: Set<number>;
    onSelect: (id: number) => void;
}) => {
    if (loading) return <Loading/>;
    if (drinks.length === 0) return <Error message="Напитки не найдены"/>;

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            {drinks.map((drink) => (
                <DrinkCard
                    key={drink.id}
                    drink={drink}
                    isSelected={selectedDrinkIds.has(drink.id!)}
                    onSelect={onSelect}
                />
            ))}
        </div>
    );
};
