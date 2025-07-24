export function getRubleWord(nominal: number): string {
    const last = nominal % 10;
    const lastTwo = nominal % 100;
    if (last === 1 && lastTwo !== 11) return 'рубль';
    if ([2, 3, 4].includes(last) && ![12, 13, 14].includes(lastTwo)) return 'рубля';
    return 'рублей';
}
