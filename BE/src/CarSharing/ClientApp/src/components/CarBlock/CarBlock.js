import React, { Component } from "react";
import './CarBlock.scss';
import Audi from '../Images/Audi.png'


export default class CarBlock extends Component {
    render() {
        return (
            <div className="car-block">

                <div className="car-content">
                    <div className="car-image">
                        <img src={Audi}/>
                    </div>

                    <div className="car-text">
                        <h2>BMW M3 2020</h2>
                        <p className="car-price"> € 60/day</p>

                        <div className="data-row">
                            <p>Transmission</p>
                            <p>Automatic</p>
                        </div>

                        <div className="data-row">
                            <p>Fuel</p>
                            <p>Diesel</p>
                        </div>

                        <div className="data-row">
                            <p>Kilometers</p>
                            <p>12000</p>
                        </div>

                        <div className="data-row">
                            <p>Seats</p>
                            <p>5</p>
                        </div>


                    </div>
                </div>
            </div>
        );
    }
}