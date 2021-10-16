import React, { Component } from "react";
import './CarFilter.scss';


export default class CarFilter extends Component {
    render() {
        return (
            <div className="filter-block">
                <h2>Brand</h2>
                <div class="container1">
                    <input type="checkbox" /> BMW <br />
                    <input type="checkbox" /> Audi <br />
                    <input type="checkbox" /> Toyota <br />
                    <input type="checkbox" /> Toyota <br />
                    <input type="checkbox" /> Toyota <br />
                    <input type="checkbox" /> Toyota <br />
                    <input type="checkbox" /> Toyota <br />
                </div>

                <h2>Fuel</h2>
                <div class="container1">
                    <input type="checkbox" /> Diesel <br />
                    <input type="checkbox" /> Benzine <br />
                    <input type="checkbox" /> Gas <br />
                </div>

                <h2>Color</h2>
                <div class="container1">
                    <input type="checkbox" /> Red <br />
                    <input type="checkbox" /> Green <br />
                    <input type="checkbox" /> Metalic <br />
                    <input type="checkbox" /> White <br />
                    <input type="checkbox" /> Black <br />
                </div>

                <button type="button">Show cars</button>
            </div>
        );
    }
}