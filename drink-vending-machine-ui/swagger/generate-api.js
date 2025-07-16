/* eslint-disable @typescript-eslint/no-require-imports */
const {generate} = require('openapi-typescript-codegen');
const path = require('path');
const fs = require('fs');

const swaggerFiles = [
    'drink-vending-machine.swagger.json'
];

const inputDir = path.resolve(__dirname, '../swagger');
const outputDir = path.resolve(__dirname, '../src/app/api');

(async () => {
    for (const file of swaggerFiles) {
        const name = file.replace('.swagger.json', '');
        const input = path.join(inputDir, file);
        const output = path.join(outputDir, name);

        if (!fs.existsSync(input)) {
            console.warn(`Swagger file not found: ${input}`);
            continue;
        }

        await generate({
            input,
            output,
            httpClient: 'fetch',
            useOptions: true,
            useUnionTypes: true,
        });

        console.log(`Generated API for: ${file}`);
    }
})();
