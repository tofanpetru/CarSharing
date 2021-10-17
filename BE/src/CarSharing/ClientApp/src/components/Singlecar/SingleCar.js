import React, { Component } from "react";
import './SingleCar.scss';

export default class SingleCar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        };
    }

    componentDidMount() {
        fetch("api/Car/Details/" + window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1))
            .then((res) => res.json())
            .then(
                (data) => {
                    this.setState({
                        data: data
                    });
                },
                (error) => {
                    console.log(error)
                }
            );
    }


    render() {
        const { data } = this.state;
        return (
            <div className="general-container">
          
                    <div>
                        <div className="single-car">
                            <div className="singlecar-container">
                                <div className="car-image">
                                    <img src={data.carImage} alt={data.title} />
                                </div>

                                <div className="car-info">
                                    <h2>{data.title}</h2>
                                    <p className="available">available</p>
                                <p className="per-day">per day<span>$ { data.pricePerDay}</span></p>
                                </div>

                            </div>
                        </div>
                        <div className="general-info">
                            <div className="general-text">
                                <h2>General Information</h2>

                                <div className="blue-line">
                                    <div></div>
                                </div>
                            </div>


                            <div className="car-details">
                                <div>
                                    <h3>Engine</h3>
                                    <p>{data.engine}</p>
                                </div>

                                <div>
                                    <h3>Kilometers</h3>
                                    <p>{data.kilometers}</p>
                                </div>

                                <div>
                                    <h3>Fuel</h3>
                                    <p>{data.fuel}</p>
                                </div>

                                <div>
                                    <h3>Seats</h3>
                                    <p>{data.seats}</p>
                                </div>

                                <div>
                                    <h3>Color</h3>
                                    <p>{data.color}</p>
                                </div>
                            </div>
                        </div>
                    </div>
             
                </div>
            );
        }
}
















