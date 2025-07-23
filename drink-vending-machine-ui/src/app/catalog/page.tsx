'use client';

import {useDrinks} from "@/hooks/use-drinks";
import {DrinkFilters} from "@/components/drinks/drink-filter";
import {DrinkList} from "@/components/drinks/drink-list";
import {DrinkImport} from "@/components/drinks/drink-import";
import {useDrinkSelection} from "@/hooks/catalog/use-drink-selection";

export default function CatalogPage() {
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
        toggleSelectDrink,
        refreshDrinks
    } = useDrinks();

    const {goToOrderPage, selectedCount} = useDrinkSelection(drinks, selectedDrinkIds);

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
                    <DrinkImport onImportSuccess={() => refreshDrinks()}/>
                    <button
                        className="bg-green-600 text-white px-4 py-3 rounded hover:bg-green-700 disabled:bg-gray-400"
                        onClick={goToOrderPage}
                        disabled={selectedCount === 0}
                    >
                        Выбрано: {selectedCount}
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
