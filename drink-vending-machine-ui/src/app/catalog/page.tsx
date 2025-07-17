'use client';

import {useDrinks} from "@/hooks/use-drinks";
import {DrinkFilters} from "@/components/drinks/drink-filter";
import {DrinkList} from "@/components/drinks/drink-list";
import {useVendingLock} from "@/hooks/use-vending-lock";
import {LockedScreen} from "@/components/ui/locked-screen";

export default function CatalogPage() {
    const { isLocked, refreshLock } = useVendingLock();

    const {
        drinks,
        brands,
        selectedBrand,
        priceBounds,
        selectedRange,
        loading,
        handleBrandChange,
        handlePriceChange,
        selectedDrinkIds,
        toggleSelectDrink
    } = useDrinks();

    if (isLocked) {
        return <LockedScreen onRefreshAAction={refreshLock} />;
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="grid md:grid-cols-[1fr_300px] gap-10 items-start">
                <div className="flex flex-col gap-6">
                    <h1 className="text-3xl font-bold">Газированные напитки</h1>
                    <DrinkFilters
                        brands={brands}
                        selectedBrand={selectedBrand}
                        priceBounds={priceBounds}
                        selectedRange={selectedRange}
                        onBrandChange={handleBrandChange}
                        onPriceChange={handlePriceChange}
                    />
                </div>
                <div className="flex flex-col space-y-7">
                    <button className="bg-gray-200 text-gray-800 px-4 py-3 rounded">
                        Импорт
                    </button>
                    <button className="bg-green-600 text-white px-4 py-3 rounded">
                        Выбрано: {selectedDrinkIds.size}
                    </button>
                </div>
            </div>
            <div className="border-t border-gray-200 my-7"></div>
            <DrinkList
                drinks={drinks}
                loading={loading}
                selectedDrinkIds={selectedDrinkIds}
                onSelect={toggleSelectDrink}
            />
        </div>
    );
}
