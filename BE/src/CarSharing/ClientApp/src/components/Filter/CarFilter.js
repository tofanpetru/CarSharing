import React, { Component } from "react";
import './CarFilter.scss';

export default class CarFilter extends Component {
    static displayName = CarFilter.name;

    constructor(props) {
        super(props);
        this.state = { brand: [], categories: [], carSpec: [] , loading: true };
    }

    componentDidMount() {
        this.populateFilterData();
    }

    static renderFiltersTable(brand, categories, carSpec) {
        return (
             <div className="filter-block">
                <h2>Brand</h2>
                <div class="container1">
                    {brand.map(brand =>
                        <div>
                            <input type="checkbox" value={brand.name} id={"brand" + brand.id} onclick="this.form.submit()" />
                            <label for={"brand" + brand.id}>{brand.name}</label>
                        </div>
                    )}
                </div>
                
                <h2>Categories</h2>
                <div class="container1">
                    {categories.map(categories =>
                        <div>
                            <input type="checkbox" value={categories.title} id={"category" + categories.id} onclick="this.form.submit()" />
                            <label for={"category" + categories.id}>{categories.title}</label>
                        </div>
                    )}
                </div>

                <h2>Color</h2>
                <div class="container1">
                    {carSpec.map(carSpec =>
                        <div>
                            <input type="checkbox" value={carSpec.color} id={"color_" + carSpec.color} onclick="this.form.submit()" />
                            <label for={"color_" + carSpec.color}>{carSpec.color}</label>
                        </div>
                    )}
                </div>

                <h2>Fuel</h2>
                <div class="container1">
                    {carSpec.map(carSpec =>
                        <div>
                            <input type="checkbox" value={carSpec.fuel} id={"fuel" + carSpec.fuel} onclick="this.form.submit()" />
                            <label for={"fuel" + carSpec.fuel}>{carSpec.fuel}</label>
                        </div>
                    )}
                </div>

                <h2>Price per day</h2>
                <div class="container1">
                    {carSpec.map(carSpec =>
                        <div>
                            <input type="checkbox" value={carSpec.pricePerDay} id={"pricePerDay" + carSpec.pricePerDay} onclick="this.form.submit()" />
                            <label for={"pricePerDay" + carSpec.pricePerDay}>{carSpec.pricePerDay}</label>
                        </div>
                    )}
                </div>

                <h2>Transmission</h2>
                <div class="container1">
                    {carSpec.map(carSpec =>
                        <div>
                            <input type="checkbox" value={carSpec.transmission} id={"transmission" + carSpec.transmission} onclick="this.form.submit()" />
                            <label for={"transmission" + carSpec.transmission}>{carSpec.transmission}</label>
                        </div>
                    )}
                </div>

                <h2>Engine</h2>
                <div class="container1">
                    {carSpec.map(carSpec =>
                        <div>
                            <input type="checkbox" value={carSpec.engine} id={"engine" + carSpec.engine} onclick="this.form.submit()" />
                            <label for={"engine" + carSpec.engine}>{carSpec.engine}</label>
                        </div>
                    )}
                </div>

                <button type="button">Show cars</button>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : CarFilter.renderFiltersTable(this.state.brand, this.state.categories, this.state.carSpec);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateFilterData() {
        const responseCarBrands = await fetch('api/CarBrands');
        const responseCategories = await fetch('api/Categories');
        const responseCarSpecifications = await fetch('api/Car/specifications');

        const dataCB = await responseCarBrands.json();
        const dataC = await responseCategories.json();
        const dataCarSpec = await responseCarSpecifications.json();

        this.setState({ brand: dataCB, categories: dataC, carSpec: dataCarSpec, loading: false });
    }
}